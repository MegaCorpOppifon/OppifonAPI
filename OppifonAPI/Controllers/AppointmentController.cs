using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.Factory;
using DAL.Models;
using DAL.Models.ManyToMany;
using Microsoft.AspNetCore.Http;
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
    public class AppointmentController : ControllerBase
    {
        private readonly IFactory _factory;

        public AppointmentController(IFactory factory)
        {
            _factory = factory;
        }

        // GET: api/Appointment
        [HttpPost]
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

        [HttpGet("{appointmentId}")]
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

        [HttpDelete("{appointmentId}")]
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

                    if (dbAppointment == null)
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

        [HttpPost("{appointmentId}/participant")]
        public IActionResult AddUserToAppointment([FromBody] DTOId dtoUserId, Guid appointmentId)
        {
            using (var unit = _factory.GetUOF())
            {
                try
                {
                    var dbAppointment = unit.Appointments.GetEager(appointmentId);
                    if (dbAppointment == null)
                        return BadRequest(new { message = $"Appointment with id '{appointmentId}' did not exist" });

                    var dbUser = unit.Users.GetEager(dtoUserId.Id);
                    if (dbUser == null)
                        return BadRequest(new { message = $"User with id '{dtoUserId.Id}' did not exist" });

                    if (dbAppointment.MaxParticipants <= dbAppointment.Participants.Count)
                        return BadRequest(new { message = "Appointment is full" });

                    if (dbAppointment.Participants.Any(x => x.Calendar.User.NormalizedEmail == dbUser.NormalizedEmail))
                        return BadRequest(new { message = "User is already in the appointment" });

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

        [HttpDelete("{appointmentId}/participant/{userId}")]
        public IActionResult RemoveUserFromAppointment(Guid appointmentId, Guid userId)
        {
            using (var unit = _factory.GetUOF())
            {
                try
                {
                    var dbAppointment = unit.Appointments.GetEager(appointmentId);
                    if (dbAppointment == null)
                        return BadRequest(new { message = $"Appointment with id '{appointmentId}' did not exist" });

                    var dbUser = unit.Users.GetEager(userId);
                    if (dbUser == null)
                        return BadRequest(new { message = $"User with id '{userId}' did not exist" });

                    var dbCalendarAppointment = unit.CalendarAppointments.Find(x =>
                        x.AppointmentId == dbAppointment.Id && x.CalendarId == dbUser.Calendar.Id).SingleOrDefault();

                    unit.CalendarAppointments.Remove(dbCalendarAppointment);
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
