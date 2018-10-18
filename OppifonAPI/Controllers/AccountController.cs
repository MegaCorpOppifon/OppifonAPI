using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DAL.Factory;
using DAL.Models;
using DAL.Models.ManyToMany;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using OppifonAPI.DTO;
using OppifonAPI.Helpers;
using Calendar = DAL.Models.Calendar;

namespace OppifonAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IFactory _factory;


        public AccountController(IFactory factory, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _factory = factory;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] DTORegisterUser dtoUser)
        {
            using (var unit = _factory.GetUOF())
            {
                try
                {
                    // Add User
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
                    var userCreationResult = await _userManager.CreateAsync(newUser, dtoUser.Password);

                    // Add user tag
                    if (userCreationResult.Succeeded)
                    {
                        var dbUser = unit.Users.GetByEmail(dtoUser.Email);
                        dbUser.InterestTags = new Collection<UserTag>();
                        foreach (var interestTag in dtoUser.InterestTags)
                        {
                            var tag = unit.Tags.GetTagByName(interestTag) ?? new Tag {Name = interestTag};

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
                            var expert = new Expert
                            {
                                Description = dtoUser.Description,
                                ExpertTags = new List<ExpertTag>(),
                                MainFields = new List<MainFieldTag>(),
                                Reviews = new List<Review>()
                            };

                            var category = unit.Categories.GetCategoryByName(dtoUser.ExpertCategory);
                            category.Tags = new List<Tag>();

                            foreach (var expertTag in dtoUser.ExpertTags)
                            {
                                var tag = unit.Tags.GetTagByName(expertTag) ?? new Tag {Name = expertTag};

                                category.Tags.Add(tag);

                                var newExpertTag = new ExpertTag
                                {
                                    Expert = expert,
                                    Tag = tag
                                };

                                expert.ExpertTags.Add(newExpertTag);
                            }

                            foreach (var expertTag in dtoUser.MainFields)
                            {
                                var tag = unit.Tags.GetTagByName(expertTag) ?? new Tag {Name = expertTag};

                                category.Tags.Add(tag);

                                var newMainField = new MainFieldTag()
                                {
                                    Expert = expert,
                                    Tag = tag
                                };

                                expert.MainFields.Add(newMainField);
                            }

                            expert.ExpertCategory = category;
                        }

                        unit.Complete();
                        var test = unit.Users.GetByEmail(dtoUser.Email);
                        var userTags = unit.UserTags.GetAll();
                        var tags = unit.Tags.GetAll();
                    }

                    if (!userCreationResult.Succeeded)
                    {
                        foreach (var error in userCreationResult.Errors)
                            ModelState.AddModelError(string.Empty, error.Description);
                        return BadRequest(ModelState);
                    }

                    //return Ok(newUser);
                    
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }

            using (var unit = _factory.GetUOF())
            {
                var test = unit.Users.GetByEmail(dtoUser.Email);
                var test2 = test.InterestTags;
                var test3 = unit.UserTags.GetAll();
                var test4 = unit.Tags.GetAll();
            }

            return Ok();
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

        [HttpPost("Logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok();
        }

        private JsonToken GenerateToken(User user)
        {
            var claims = new[]
            {
                new Claim("email", user.Email),
                new Claim("name", user.FirstName + " " + user.LastName), 
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
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes("the secret that needs to be at least 16 characters long for HmacSha256")),
                    SecurityAlgorithms.HmacSha256)),
                new JwtPayload(claims));
            var writtenToken = new JwtSecurityTokenHandler().WriteToken(token);
            return new JsonToken(writtenToken);
        }
    }
}
