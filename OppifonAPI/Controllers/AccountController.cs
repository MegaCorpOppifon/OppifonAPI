﻿using DAL.Factory;
using DAL.Models;
using DAL.Models.ManyToMany;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using OppifonAPI.DTO;
using OppifonAPI.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Calendar = DAL.Models.Calendar;

namespace OppifonAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly IFactory _factory;


        public AccountController(IFactory factory, UserManager<User> userManager, SignInManager<User> signInManager, IConfiguration configuration)
        {
            _factory = factory;
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] DTORegisterUser dtoUser)
        {
            using (var unit = _factory.GetUOF())
            {
                try
                {
                    // Add User
                    IdentityResult userCreationResult;
                    if (dtoUser.IsExpert)
                    {
                        var newUser = new Expert
                        {
                            UserName = dtoUser.Email,
                            Email = dtoUser.Email,
                            City = dtoUser.City,
                            Birthday = dtoUser.Birthday,
                            FirstName = dtoUser.FirstName,
                            LastName = dtoUser.LastName,
                            Gender = dtoUser.Gender,
                            IsExpert = dtoUser.IsExpert,
                            PhoneNumber = dtoUser.PhoneNumber,
                            Calendar = new Calendar(),
                            Description = dtoUser.Description,
                            ExpertTags = new List<ExpertTag>(),
                            MainFields = new List<MainFieldTag>(),
                            Reviews = new List<Review>()
                        };
                        userCreationResult = await _userManager.CreateAsync(newUser, dtoUser.Password);
                    }
                    else
                    {
                        var newUser = new User
                        {
                            UserName = dtoUser.Email,
                            Email = dtoUser.Email,
                            City = dtoUser.City,
                            Birthday = dtoUser.Birthday,
                            FirstName = dtoUser.FirstName,
                            LastName = dtoUser.LastName,
                            Gender = dtoUser.Gender,
                            IsExpert = dtoUser.IsExpert,
                            PhoneNumber = dtoUser.PhoneNumber,
                            Calendar = new Calendar()
                        };
                        userCreationResult = await _userManager.CreateAsync(newUser, dtoUser.Password);
                    }


                    // Add user tag
                    if (userCreationResult.Succeeded)
                    {
                        var dbUser = unit.Users.GetByEmail(dtoUser.Email);
                        dbUser.InterestTags = new Collection<UserTag>();

                        if (dtoUser.InterestTags == null)
                            dtoUser.InterestTags = new Collection<string>();

                        foreach (var interestTag in dtoUser.InterestTags)
                        {
                            var tag = unit.Tags.GetTagByName(interestTag) ?? new Tag { Name = interestTag };

                            var userTag = new UserTag
                            {
                                Tag = tag,
                                User = dbUser
                            };

                            dbUser.InterestTags.Add(userTag);
                        }

                        unit.Complete();

                        if (dtoUser.IsExpert)
                        {
                            var dbExpert = unit.Experts.GetByEmail(dtoUser.Email);
                            dbExpert.ExpertTags = new List<ExpertTag>();
                            dbExpert.MainFields = new List<MainFieldTag>();
                            var category = unit.Categories.GetCategoryEagerByName(dtoUser.ExpertCategory);

                            if (dtoUser.ExpertTags == null)
                                return BadRequest(new { message = "You must have some expert tags" });

                            foreach (var expertTag in dtoUser.ExpertTags)
                            {
                                var tag = unit.Tags.GetTagByName(expertTag) ?? new Tag { Name = expertTag };

                                category.Tags.Add(tag);

                                var newExpertTag = new ExpertTag
                                {
                                    Expert = dbExpert,
                                    Tag = tag
                                };

                                dbExpert.ExpertTags.Add(newExpertTag);
                            }

                            unit.Complete();

                            if (dtoUser.MainFields == null)
                                return BadRequest(new { message = "You must have some main fields" });

                            foreach (var expertTag in dtoUser.MainFields)
                            {
                                var tag = unit.Tags.GetTagByName(expertTag) ?? new Tag { Name = expertTag };

                                category.Tags.Add(tag);

                                var newMainField = new MainFieldTag()
                                {
                                    Expert = dbExpert,
                                    Tag = tag
                                };

                                dbExpert.MainFields.Add(newMainField);
                            }

                            dbExpert.ExpertCategory = category;
                        }

                        unit.Complete();
                    }

                    if (userCreationResult.Succeeded)
                        return Ok();

                    foreach (var error in userCreationResult.Errors)
                        ModelState.AddModelError(string.Empty, error.Description);
                    return BadRequest(ModelState);

                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
        }

        [HttpPost("AddProfilePicture/{id}")]
        public async Task<IActionResult> AddProfilePicture([FromForm] DTOImage image, Guid id)
        {
            if (image.Image == null || image.Image.Length == 0)
                return Content("file not selected");

            var path = Path.Combine(
                        Directory.GetCurrentDirectory(), "wwwroot/profileImages",
                        image.Image.FileName);
            using (var stream = new FileStream(path, FileMode.Create))
            {
                await image.Image.CopyToAsync(stream);

                using (var unit = _factory.GetUOF())
                {
                    try
                    {
                        // find user
                        var currentAppuser = unit.Users.Get(id);
                        // Add Image
                        // TODO change connectionstring for production
                        string conString =   ConfigurationExtensions
                                             .GetConnectionString(_configuration, "profileImages");
                        currentAppuser.Image = conString + "/" + image.Image.FileName;
                        unit.Complete();
                        return Ok();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        throw;
                    }
                }
            }
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody]DTOLoginUser dtoUser)
        {
            var user = await _userManager.FindByEmailAsync(dtoUser.Email);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid login");
                return BadRequest(ModelState);
            }
            var passwordSignInResult = await _signInManager.CheckPasswordSignInAsync(user, dtoUser.Password, false);
            if (passwordSignInResult.Succeeded)
                return Ok(GenerateToken(user));
            return BadRequest("Invalid login");
        }

        private JsonToken GenerateToken(User user)
        {
            var claims = new[]
            {
                new Claim("email", user.Email),
                new Claim("firstName", user.FirstName),
                new Claim("lastName", user.LastName), 
                new Claim("id", user.Id.ToString()), 
                new Claim("city", user.City), 
                new Claim("birthday", user.Birthday.ToString(CultureInfo.InvariantCulture)),
                new Claim("gender", user.Gender), 
                new Claim("isExpert", user.IsExpert.ToString()), 
                new Claim(JwtRegisteredClaimNames.Nbf, new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds().ToString()),
                new Claim(JwtRegisteredClaimNames.Exp, new DateTimeOffset(DateTime.Now.AddDays(1)).ToUnixTimeSeconds().ToString()),
            };
            
            var token = new JwtSecurityToken(
                new JwtHeader(new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes("AKSE5aYnyjEXs5eSQbYrHdaW6QEXWXGfxKfe9MxhPxyFpg2bghzFNAX7Wu4xrhExeZFBdm6Qzz85sDMWptWWJp7Jz6pwr9w2GTeP3MJer7M8kjKkzZWcdBGJ")),
                    SecurityAlgorithms.HmacSha256)),
                new JwtPayload(claims));
            var writtenToken = new JwtSecurityTokenHandler().WriteToken(token);
            return new JsonToken(writtenToken);
        }
    }
}
