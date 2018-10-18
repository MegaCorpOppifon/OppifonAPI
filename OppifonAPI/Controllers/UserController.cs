using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OppifonAPI.Helpers;
using DAL.Factory;
using DAL.Models;
using DAL.Models.ManyToMany;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Internal;
using OppifonAPI.DTO;

namespace OppifonAPI.Controllers
{
    [ApiController]
    //[Authorize]
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
            using (var unit = _factory.GetUOF())
            {
                var user = unit.Users.Get(id);
                var dtoUser = new DTORegisterUser();
                Mapper.Map(user, dtoUser);
                return Ok(dtoUser);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody]DTOUpdateUser dtoUser)
        {
            using (var unit = _factory.GetUOF())
            {
                // map dto to entity and set id
                try
                {
                    var dbUser = unit.Users.Get(id);
                    await _userManager.ChangePasswordAsync(dbUser, dtoUser.OldPassword, dtoUser.NewPassword);
                    dbUser.FirstName = dtoUser.FirstName ?? dbUser.FirstName;
                    dbUser.LastName = dtoUser.LastName ?? dbUser.LastName;
                    dbUser.Email = dtoUser.Email ?? dbUser.Email;
                    dbUser.City = dtoUser.City ?? dbUser.City;
                    dbUser.PhoneNumber = dtoUser.PhoneNumber ?? dbUser.PhoneNumber;
                    dbUser.Birthday = dtoUser.Birthday == default(DateTime) ? dtoUser.Birthday : dbUser.Birthday;
                    dbUser.Gender = dtoUser.Gender ?? dbUser.Gender;
                    dbUser.IsExpert = dtoUser.IsExpert;

                    // Interest tags
                    if (dtoUser.InterestTags.Any())
                    {
                        dbUser.InterestTags = new Collection<UserTag>();
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
                    }
                    
                    // save 
                    unit.Complete();
                    return Ok(dbUser);
                }
                catch (AppException ex)
                {
                    // return error message if there was an exception
                    return BadRequest(new { message = ex.Message });
                }
            }
        }
    }
}
