using System;
using System.Collections.Generic;
using System.Linq;
using DAL.Factory;
using DAL.Models;
using DAL.Models.ManyToMany;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OppifonAPI.DTO;
using OppifonAPI.DTO.Calendar;
using OppifonAPI.Helpers;

namespace OppifonAPI.Controllers
{
    [ApiController]
    [Authorize]
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
        public IActionResult GetCalendarUser(Guid id)
        {
            //var claims = User.Claims;
            //var isExpert = claims.FirstOrDefault(x => x.Type == "isExpert")?.Value;
            //if (isExpert != "True")
            //    return Unauthorized();

            using (var unit = _factory.GetUOF())
            {
                try
                {
                    var dbCalendar = unit.Calendars.GetEagerByUserId(id);
                    if (dbCalendar == null)
                        return BadRequest(new {message = $"No calendar was found with id '{id}'"});

                    var dtoCalendar = new DTOCalendarPrivate
                    {
                        Id = dbCalendar.Id,
                        DefaultDuration = dbCalendar.DefaultDuration,
                        WorkDays = dbCalendar.WorkDays,
                        DaysOff = dbCalendar.DaysOff,
                        Appointments = new List<DTOAppointmentPrivate>()
                    };

                    foreach (var appointment in dbCalendar.Appointments)
                    {
                        var dtoAppointment = new DTOAppointmentPrivate
                        {
                            StartTime = appointment.Appointment.StartTime,
                            EndTime = appointment.Appointment.EndTime,
                            Id = appointment.AppointmentId,
                            Text = appointment.Appointment.Text,
                            Title = appointment.Appointment.Title,
                            MaxParticipants = appointment.Appointment.MaxParticipants,
                            Participants = new List<DTOUser>()
                        };

                        foreach (var participant in appointment.Appointment.Participants)
                        {
                            var user = participant.Calendar.User;
                            dtoAppointment.Participants.Add(new DTOUser
                            {
                                Id = user.Id,
                                InterestTags = new List<string>(),
                                IsExpert = user.IsExpert,
                                Email = user.Email,
                                City = user.City,
                                Gender = user.Gender,
                                Birthday = user.Birthday,
                                LastName = user.LastName,
                                FirstName = user.FirstName,
                                PhoneNumber = user.PhoneNumber
                            });
                        }

                        dtoCalendar.Appointments.Add(dtoAppointment);
                    }

                    return Ok(dtoCalendar);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
        }

        [HttpGet("expert/{id}")]
        public IActionResult GetCalendarExpert(Guid id)
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
                                StartTime = appointment.Appointment.StartTime,
                                EndTime = appointment.Appointment.EndTime
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

       

       
    }
}
