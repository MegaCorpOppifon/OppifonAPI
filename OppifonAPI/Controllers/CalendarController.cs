using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.Factory;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace OppifonAPI.Controllers
{
    [ApiController]
    //[Authorize]
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class CalendarController : ControllerBase
    {
        private IFactory _factory;

        public CalendarController(IFactory factory)
        {
            _factory = factory;
        }

        #region user

        [HttpGet("user/{id}")]
        public IActionResult GetCalendarUser(Guid id)
        {
            using (var unit = _factory.GetUOF())
            {
                try
                {
                    var calendar = unit.Users.GetEager(id).Calendar;
                    return Ok(calendar);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
        }

        #endregion
    }
}
