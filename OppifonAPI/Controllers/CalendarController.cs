using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.Factory;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OppifonAPI.DTO;

namespace OppifonAPI.Controllers
{
    [ApiController]
    //[Authorize]
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class CalendarController : ControllerBase
    {
        private readonly IFactory _factory;

        public CalendarController(IFactory factory)
        {
            _factory = factory;
        }

        #region user

        [HttpGet("user/{id}")]
        public IActionResult GetUserCalendar(Guid id)
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

        #region expert

        [HttpGet("expert/{id}")]
        public IActionResult GetExpertCalendar(Guid id)
        {
            using (var unit = _factory.GetUOF())
            {
                try
                {
                    var calendar = new DTOCalendarPublic();

                    var dbCalendar = unit.Users.GetEager(id).Calendar;

                    calendar.Id = dbCalendar.Id;
                    calendar.DaysOff = dbCalendar.DaysOff;
                    calendar.DefaultDuration = dbCalendar.DefaultDuration;
                    calendar.WorkDays = dbCalendar.WorkDays;
                    calendar.Appointments = new List<DTOAppointmentPublic>();

                    foreach (var appointment in dbCalendar.Appointments)
                    {
                        calendar.Appointments.Add(
                            new DTOAppointmentPublic
                            {
                                Time = appointment.Appointment.Time,
                                Duration = appointment.Appointment.Duration
                            });
                    }

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
