using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using OppifonAPI.Helpers;
using DAL.Factory;
using Microsoft.AspNetCore.Authorization;
using OppifonAPI.DTO;

namespace OppifonAPI.Controllers
{
    [ApiController]
    //[Authorize]
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IFactory _factory;
        
        public UsersController(IFactory factory)
        {
            _factory = factory;
        }

        [HttpGet("{id}")]
        public IActionResult GetById(Guid id)
        {
            using (var unit = _factory.GetUOF())
            {
                var user = unit.Users.Get(id);
                var dtoUser = new DTORegisterUser();
                Mapper.Map(user, dtoUser);
                return Ok(dtoUser);
            }
        }

        [HttpPut]
        public IActionResult GetByEmail([FromForm] DTOLoginUser dtoUser)
        {
            using (var unit = _factory.GetUOF())
            {
                var user = unit.Users.GetByEmail(dtoUser.Email);
                var newDtoUser = new DTORegisterUser();
                Mapper.Map(user, newDtoUser);
                return Ok(newDtoUser);
            }
        }


        //[HttpPut("{id}")]
        //public IActionResult Update(Guid id, [FromBody]DTOUser dtoUser)
        //{
        //    using (var unit = _factory.GetUOF())
        //    {
        //        // map dto to entity and set id
        //        try
        //        {
        //            dtoUser.Id = id;
        //            var user = unit.Users.GetByEmail(dtoUser.Email);
        //            Mapper.Map(dtoUser, user);

        //            // save 
        //            unit.Users.Update(user);
        //            unit.Complete();
        //            return Ok(user);
        //        }
        //        catch (AppException ex)
        //        {
        //            // return error message if there was an exception
        //            return BadRequest(new { message = ex.Message });
        //        }
        //    }
        //}
    }
}
