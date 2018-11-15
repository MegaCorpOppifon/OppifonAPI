using System;
using System.Collections.Generic;
using System.Linq;
using DAL.Factory;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OppifonAPI.DTO;
using OppifonAPI.DTO.Calendar;
using OppifonAPI.Helpers;

namespace OppifonAPI.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class CalendarController : ControllerBase
    {
        private readonly IFactory _factory;

        public CalendarController(IFactory factory)
        {
            _factory = factory;
        }

        [HttpGet("user/{id}")]
        [Authorize]
        public ActionResult<DTOCalendarPrivate> GetCalendarUser(Guid id)
        {
            var claims = User.Claims;
            var userId = claims.FirstOrDefault(x => x.Type == "id")?.Value;
            if (userId != id.ToString())
                return Unauthorized();

            using (var unit = _factory.GetUOF())
            {
                try
                {
                    var dbCalendar = unit.Calendars.GetEagerByUserId(id);
                    if (dbCalendar == null)
                        return BadRequest(new {message = $"No calendar was found with id '{id}'"});

                    var dtoCalendar = new DTOCalendarPrivate
                    {
                       Appointments = new List<DTOAppointmentPrivate>()
                    };

                    Mapper.Map(dbCalendar, dtoCalendar);

                    foreach (var appointment in dbCalendar.Appointments)
                    {
                        var dtoAppointment = new DTOAppointmentPrivate
                        {
                           Participants = new List<DTOSimpleUser>()
                        };
                        Mapper.Map(appointment.Appointment, dtoAppointment);

                        foreach (var participant in appointment.Appointment.Participants)
                        {
                            var user = participant.Calendar.User;
                            var dtoUser = new DTOSimpleUser();
                            Mapper.Map(user, dtoUser);
                            dtoAppointment.Participants.Add(dtoUser);
                            
                        }

                        dtoCalendar.Appointments.Add(dtoAppointment);
                    }

                    return dtoCalendar;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
        }

        [HttpGet("expert/{id}")]
        public ActionResult<DTOCalendarPublic> GetCalendarExpert(Guid id)
        {
            using (var unit = _factory.GetUOF())
            {
                try
                {
                    var dbCalendar = unit.Calendars.GetEagerByUserId(id);

                    if (dbCalendar == null)
                        return BadRequest(new { message = $"No expert was found with id: {id}" });
                   
                    var calendar = new DTOCalendarPublic
                    {
                        Appointments = new List<DTOAppointmentPublic>()
                    };
                    Mapper.Map(dbCalendar, calendar);

                    foreach (var appointment in dbCalendar.Appointments)
                    {
                        calendar.Appointments.Add(
                            new DTOAppointmentPublic
                            {
                                StartTime = appointment.Appointment.StartTime,
                                EndTime = appointment.Appointment.EndTime
                            });
                    }

                    return calendar;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
        }
    }
}
