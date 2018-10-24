using System;
using System.Collections.Generic;
using System.Linq;
using DAL.Factory;
using DAL.Models;
using DAL.Models.ManyToMany;
using Microsoft.AspNetCore.Mvc;
using OppifonAPI.DTO;
using OppifonAPI.Helpers;

namespace OppifonAPI.Controllers
{
    [ApiController]
    //[Authorize]
    [Produces("application/json")]
    [Route("api/calendar/[controller]")]
    public class AppointmentController : ControllerBase
    {
        private readonly IFactory _factory;

        public AppointmentController(IFactory factory)
        {
            _factory = factory;
        }

        [HttpPut("appointment/{appointmentId}")]
        public IActionResult AddUserToAppointment([FromBody] DTOUser dtoUser, Guid appointmentId)
        {
            using (var unit = _factory.GetUOF())
            {
                try
                {
                    var dbAppointment = unit.Appointments.GetEager(appointmentId);

                    var dbUser = dtoUser.Email == null ? unit.Users.GetEager(dtoUser.Id) : unit.Users.GetEagerByEmail(dtoUser.Email);

                    if (dbAppointment.MaxParticipants <= dbAppointment.Participants.Count)
                        return BadRequest(new { message = "Appointment is full" });

                    if (dbAppointment.Participants.Any(x => x.NormalizedEmail == dbUser.NormalizedEmail))
                        return BadRequest(new { message = "User is already in the appointment" });

                    var calendarAppointment = new CalendarAppointment
                    {
                        Appointment = dbAppointment,
                        Calendar = dbUser.Calendar
                    };

                    dbUser.Calendar.Appointments.Add(calendarAppointment);
                    dbAppointment.Participants.Add(dbUser);
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

        [HttpPost("{userId}/appointment")]
        public IActionResult AddAppointment([FromBody] DTOAppointment dtoAppointment, Guid userId)
        {
            using (var unit = _factory.GetUOF())
            {
                try
                {
                    var dbExpert = unit.Experts.GetEager(userId);

                    // Check if there is a spot in the calendar
                    var freeAppointment = dbExpert.Calendar.Appointments.Any(x =>
                        x.Appointment.Time <= dtoAppointment.Time &&
                        dtoAppointment.Time >= x.Appointment.Time.Add(x.Appointment.Duration));

                    if (freeAppointment)
                        return BadRequest(new { message = "Please pick a free spot in the calendar" });

                    // Create appointment
                    var appointment = new Appointment
                    {
                        Participants = new List<User>(),
                        Duration = dtoAppointment.Duration,
                        Time = dtoAppointment.Time,
                        Text = dtoAppointment.Text
                    };
                    appointment.Participants.Add(dbExpert);

                    // If expert is the creator, then this is an event
                    if (dtoAppointment.Email == dbExpert.Email)
                    {
                        appointment.MaxParticipants = dtoAppointment.MaxParticipants;
                    }
                    else
                    {
                        // A user cannot have more than two participants in an appointment
                        appointment.MaxParticipants = 2;

                        // Get user
                        var participator = unit.Users.GetEagerByEmail(dtoAppointment.Email);
                        appointment.Participants.Add(participator);

                        var userCalendarAppointment = new CalendarAppointment
                        {
                            Appointment = appointment,
                            Calendar = participator.Calendar
                        };

                        participator.Calendar.Appointments.Add(userCalendarAppointment);
                    }

                    var expertCalendarAppointment = new CalendarAppointment
                    {
                        Appointment = appointment,
                        Calendar = dbExpert.Calendar
                    };

                    dbExpert.Calendar.Appointments.Add(expertCalendarAppointment);
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
        public IActionResult GetAppointmentUser(Guid appointmentId)
        {
            using (var unit = _factory.GetUOF())
            {
                var dbAppointment = unit.Appointments.GetEager(appointmentId);
                if (dbAppointment == null)
                    return BadRequest(new { message = $"No appointment existed with the id '{appointmentId}'" });

                DTOAppointmentPrivate dtoAppointment = new DTOAppointmentPrivate
                {
                    Participants = new List<DTOUser>()
                };

                Mapper.Map(dbAppointment, dtoAppointment);

                foreach (var participant in dbAppointment.Participants)
                {
                    DTOUser dtoUser = new DTOUser();
                    Mapper.Map(participant, dtoUser);
                    dtoAppointment.Participants.Add(dtoUser);
                }

                return Ok(dtoAppointment);
            }
        }

    }
}
