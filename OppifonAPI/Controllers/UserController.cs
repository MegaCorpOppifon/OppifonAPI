﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DAL.Factory;
using DAL.Models;
using DAL.Models.ManyToMany;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using OppifonAPI.DTO;
using OppifonAPI.Helpers;

namespace OppifonAPI.Controllers
{
    [ApiController]
    [Authorize]
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IFactory _factory;
        private readonly UserManager<User> _userManager;

        public UserController(IFactory factory, UserManager<User> userManager)
        {
            _factory = factory;
            _userManager = userManager;
        }

        [HttpGet("{id}")]
        public IActionResult Get(Guid id)
        {
            var claims = User.Claims;
            var userId = claims.FirstOrDefault(x => x.Type == "id")?.Value;
            if (userId != id.ToString())
                return Unauthorized();

            using (var unit = _factory.GetUOF())
            {
                var dbUser = unit.Users.GetEager(id);
                var user = new DTOUser
                {
                    InterestTags = new List<string>(),
                    Favorites = new List<DTOSimpleUser>()
                };
                Mapper.Map(dbUser, user);

                foreach (var interestTag in dbUser.InterestTags)
                {
                    user.InterestTags.Add(interestTag.Tag.Name);
                }

                foreach (var favorite in dbUser.Favorites)
                {
                    var dtoSimpleUser = new DTOSimpleUser();
                    Mapper.Map(favorite.Expert, dtoSimpleUser);
                }

                return Ok(user);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] DTOUpdateUser dtoUser)
        {
            var claims = User.Claims;
            var userId = claims.FirstOrDefault(x => x.Type == "id")?.Value;
            if (userId != id.ToString())
                return Unauthorized();

            using (var unit = _factory.GetUOF())
            {
                try
                {
                    var dbUser = unit.Users.GetEager(id);

                    dbUser.FirstName = dtoUser.FirstName ?? dbUser.FirstName;
                    dbUser.LastName = dtoUser.LastName ?? dbUser.LastName;
                    dbUser.Email = dtoUser.Email ?? dbUser.Email;
                    dbUser.UserName = dtoUser.Email ?? dbUser.UserName;
                    dbUser.City = dtoUser.City ?? dbUser.City;
                    dbUser.PhoneNumber =
                        dtoUser.PhoneNumber ?? dbUser.PhoneNumber;
                    dbUser.Birthday = dtoUser.Birthday == default(DateTime)
                        ? dtoUser.Birthday
                        : dbUser.Birthday;
                    dbUser.Gender = dtoUser.Gender ?? dbUser.Gender;
                    dbUser.IsExpert = dtoUser.IsExpert;

                    // Interest tags
                    if (dtoUser.InterestTags != null)
                    {
                        dbUser.InterestTags = new Collection<UserTag>();
                        foreach (var interestTag in dtoUser.InterestTags)
                        {
                            var tag = unit.Tags.GetTagByName(interestTag) ??
                                      new Tag {Name = interestTag};

                            var userTag = new UserTag
                            {
                                Tag = tag,
                                User = dbUser
                            };

                            dbUser.InterestTags.Add(userTag);
                        }
                    }

                    // save 
                    unit.Complete();
                }
                catch (Exception ex)
                {
                    // return error message if there was an exception
                    return BadRequest(new {message = ex.Message});
                }
            }

            try
            {
                var dbUser = await _userManager.FindByIdAsync(id.ToString());
                await _userManager.ChangePasswordAsync(dbUser,
                    dtoUser.OldPassword, dtoUser.NewPassword);
            }
            catch (Exception ex)
            {
                return BadRequest(new {message = ex.Message});
            }

            return Ok();
        }

        [HttpGet("{userId}/favorites", Name = "GetFavorites")]
        public ActionResult<List<DTOSimpleUser>> GetFavorites(Guid userId)
        {
            var claims = User.Claims;
            var id = claims.FirstOrDefault(x => x.Type == "id")?.Value;
            if (id != userId.ToString())
                return Unauthorized();

            using (var unit = _factory.GetUOF())
            {
                var dbFavorites = unit.UserFavorites.GetFavorites(userId);

                var list = new List<DTOSimpleUser>();
                foreach (var favorite in dbFavorites)
                {
                    var dtoSimpleUser = new DTOSimpleUser();
                    Mapper.Map(favorite.Expert, dtoSimpleUser);
                    list.Add(dtoSimpleUser);
                }

                return list;
            }
        }

        [HttpPost("{userId}/favorites")]
        public ActionResult<DTOSimpleUser> AddFavorite([FromBody] DTOId expertId, Guid userId)
        {
            var claims = User.Claims;
            var id = claims.FirstOrDefault(x => x.Type == "id")?.Value;
            if (id != userId.ToString())
                return Unauthorized();

            using (var unit = _factory.GetUOF())
            {
                var dbUser = unit.Users.GetEager(userId);
                var dbExpert = unit.Experts.Get(expertId.Id);

                if (dbExpert == null)
                    return BadRequest(new {message = $"Expert with id: {expertId} does not exist"});

                if (dbUser.Favorites.Any(x => x.ExpertId == expertId.Id))
                    return BadRequest(new {message = $"Favorite with id {expertId.Id}, is already in list"});

                var userFavorite = new UserFavorites
                {
                    Expert = dbExpert,
                    User = dbUser
                };

                dbUser.Favorites.Add(userFavorite);

                unit.Complete();

                var simpleUser = new DTOSimpleUser();
                Mapper.Map(dbUser, simpleUser);
                return CreatedAtAction("GetFavorites", simpleUser);
            }
        }

        [HttpDelete("{userId}/favorites/{expertId}")]
        public IActionResult RemoveFavorite(Guid userId, Guid expertId)
        {
            var claims = User.Claims;
            var id = claims.FirstOrDefault(x => x.Type == "id")?.Value;
            if (id != userId.ToString())
                return Unauthorized();

            using (var unit = _factory.GetUOF())
            {
                var dbUser = unit.Users.GetEager(userId);
                var expert = dbUser.Favorites.SingleOrDefault(x => x.ExpertId == expertId);

                if (expert == null)
                    return BadRequest(new {message = $"No expert found in favorites list with id {expertId}"});

                dbUser.Favorites.Remove(expert);

                unit.Complete();
            }

            return Ok();
        }

        [AllowAnonymous]
        [HttpPost("ForgotPassword")]
        public IActionResult ForgotPassword([FromBody] DTOEmail email)
        {            
            return Ok();
        }

    }
}