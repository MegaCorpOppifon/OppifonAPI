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
                            Duration = appointment.Appointment.Duration,
                            Time = appointment.Appointment.Time,
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

        [HttpPost("appointment")]
        public IActionResult AddAppointment([FromBody] DTOAppointment dtoAppointment)
        {
            using (var unit = _factory.GetUOF())
            {
                try
                {
                    var dbUser = unit.Users.GetEager(dtoAppointment.CreatorId);
                    
                    // Check if there is a spot in the calendar
                    var freeAppointment = dbUser.Calendar.Appointments.Any(x =>
                         x.Appointment.Time <= dtoAppointment.Time &&
                         dtoAppointment.Time >= x.Appointment.Time.Add(x.Appointment.Duration));

                    if (freeAppointment)
                        return BadRequest(new { message = "Please pick a free spot in the calendar" });

                    // Create appointment
                    var appointment = new Appointment
                    {
                        Participants = new List<CalendarAppointment>(),
                        Duration = dtoAppointment.Duration,
                        Time = dtoAppointment.Time,
                        Text = dtoAppointment.Text,
                        MaxParticipants = dtoAppointment.MaxParticipants
                    };
                    
                    var calendarAppointment = new CalendarAppointment
                    {
                        Appointment = appointment,
                        Calendar = dbUser.Calendar
                    };

                    dbUser.Calendar.Appointments.Add(calendarAppointment);
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

        [HttpPut("appointment/{appointmentId}")]
        public IActionResult AddUserToAppointment([FromBody] DTOId dtoUserId, Guid appointmentId)
        {
            using (var unit = _factory.GetUOF())
            {
                try
                {
                    var dbAppointment = unit.Appointments.GetEager(appointmentId);
                    if (dbAppointment == null)
                        return BadRequest(new {message = $"Appointment with id '{appointmentId}' did not exist"});

                    var dbUser = unit.Users.GetEager(dtoUserId.Id);
                    if(dbUser == null)
                        return BadRequest(new { message = $"User with id '{dtoUserId.Id}' did not exist" });

                    if (dbAppointment.MaxParticipants <= dbAppointment.Participants.Count)
                        return BadRequest(new {message = "Appointment is full"});

                    if (dbAppointment.Participants.Any(x => x.Calendar.User.NormalizedEmail == dbUser.NormalizedEmail))
                        return BadRequest(new {message = "User is already in the appointment"});
                    
                    var calendarAppointment = new CalendarAppointment
                    {
                        Appointment = dbAppointment,
                        Calendar = dbUser.Calendar
                    };

                    dbAppointment.Participants.Add(calendarAppointment);
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
        
        [HttpGet("appointment/{appointmentId}")]
        public IActionResult GetAppointment(Guid appointmentId)
        {
            using (var unit = _factory.GetUOF())
            {
                var dbAppointment = unit.Appointments.GetEager(appointmentId);
                if (dbAppointment == null)
                    return BadRequest(new { message = $"Appointment with id '{appointmentId}' did not exist" });

                DTOAppointmentPrivate dtoAppointment = new DTOAppointmentPrivate
                {
                    Participants = new List<DTOUser>()
                };

                Mapper.Map(dbAppointment, dtoAppointment);

                foreach (var participant in dbAppointment.Participants)
                {
                    
                    DTOUser dtoUser = new DTOUser();
                    Mapper.Map(participant.Calendar.User, dtoUser);
                    dtoAppointment.Participants.Add(dtoUser);
                }

                return Ok(dtoAppointment);
            }
        }

        [HttpDelete("appointment/{appointmentId}")]
        public IActionResult DeleteAppointment(Guid appointmentId)
        {
            //var claims = User.Claims;
            //var isExpert = claims.FirstOrDefault(x => x.Type == "isExpert")?.Value;
            //if (isExpert != "True")
            //    return Unauthorized();

            using (var unit = _factory.GetUOF())
            {
                try
                {
                    var dbAppointment = unit.Appointments.Get(appointmentId);

                    if(dbAppointment == null)
                        return BadRequest(new { message = $"Appointment with id '{appointmentId}' did not exist" });

                    unit.Appointments.Remove(dbAppointment);
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
}
