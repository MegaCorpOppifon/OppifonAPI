using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.Factory;
using DAL.Models;
using DAL.Models.ManyToMany;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
    public class AppointmentController : ControllerBase
    {
        private readonly IFactory _factory;

        public AppointmentController(IFactory factory)
        {
            _factory = factory;
        }

        [HttpPost]
        public ActionResult<Appointment> AddAppointment([FromBody] DTOAppointment dtoAppointment)
        {
            var claims = User.Claims;
            var userId = claims.FirstOrDefault(x => x.Type == "id")?.Value;
            if (userId != dtoAppointment.OwnerId.ToString())
                return Unauthorized();

            using (var unit = _factory.GetUOF())
            {
                try
                {
                    var dbUser = unit.Users.GetEager(dtoAppointment.OwnerId);
                    var startTime = StringToDateTime.Convert(dtoAppointment.StartTime);
                    var endTime = StringToDateTime.Convert(dtoAppointment.EndTime);

                    // Create appointment
                    var appointment = new Appointment
                    {
                        Participants = new List<CalendarAppointment>(),
                        StartTime = startTime,
                        EndTime = endTime,
                        Text = dtoAppointment.Text,
                        Title = dtoAppointment.Title,
                        MaxParticipants = dtoAppointment.MaxParticipants,
                        OwnerId = dtoAppointment.OwnerId
                    };

                    //Check if there is a spot in the calendar
                    var occupiedTime = dbUser.Calendar.Appointments.Any(x => x.Appointment.StartTime <= appointment.StartTime &&
                         x.Appointment.EndTime >= appointment.StartTime);

                    if (occupiedTime)
                        return BadRequest(new { message = "Please pick a free spot in the calendar" });

                    var calendarAppointment = new CalendarAppointment
                    {
                        Appointment = appointment,
                        Calendar = dbUser.Calendar
                    };

                    dbUser.Calendar.Appointments.Add(calendarAppointment);
                    unit.Complete();

                    var res = new DTOAppointmentPrivate
                    {
                        Participants = new List<DTOSimpleUser>()
                    };
                    var simpleUser = new DTOSimpleUser();
                    Mapper.Map(dbUser, simpleUser);
                    res.Participants.Add(simpleUser);

                    Mapper.Map(appointment, res);

                    return CreatedAtAction("GetAppointment", new { appointmentId = res.Id}, res);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
        }

        [HttpGet("{appointmentId}", Name = "GetAppointment")]
        public ActionResult<DTOAppointmentPrivate> GetAppointment(Guid appointmentId)
        {
            using (var unit = _factory.GetUOF())
            {
                var dbAppointment = unit.Appointments.GetEager(appointmentId);
                if (dbAppointment == null)
                    return BadRequest(new { message = $"Appointment with id '{appointmentId}' did not exist" });

                DTOAppointmentPrivate dtoAppointment = new DTOAppointmentPrivate
                {
                    Participants = new List<DTOSimpleUser>()
                };

                Mapper.Map(dbAppointment, dtoAppointment);

                var claims = User.Claims;
                var userId = claims.FirstOrDefault(x => x.Type == "id")?.Value;
                if (userId != dbAppointment.OwnerId.ToString())
                    return Unauthorized();

                foreach (var participant in dbAppointment.Participants)
                {

                    var dtoUser = new DTOSimpleUser();
                    Mapper.Map(participant.Calendar.User, dtoUser);
                    dtoAppointment.Participants.Add(dtoUser);
                }

                return dtoAppointment;
            }
        }

        [HttpDelete("{appointmentId}")]
        public IActionResult DeleteAppointment(Guid appointmentId)
        {
            using (var unit = _factory.GetUOF())
            {
                try
                {
                    var dbAppointment = unit.Appointments.Get(appointmentId);

                    if (dbAppointment == null)
                        return BadRequest(new
                        {
                            message = $"Appointment with id '{appointmentId}' did not exist"
                        });

                    var claims = User.Claims;
                    var userId = claims.FirstOrDefault(x => x.Type == "id")?.Value;
                    if (userId != dbAppointment.OwnerId.ToString())
                        return Unauthorized();

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
        public ActionResult<DTOAppointmentPrivate> AddUserToAppointment([FromBody] DTOId userId, Guid appointmentId)
        {
            using (var unit = _factory.GetUOF())
            {
                try
                {
                    var dbAppointment = unit.Appointments.GetEager(appointmentId);
                    if (dbAppointment == null)
                        return BadRequest(new { message = $"Appointment with id '{appointmentId}' did not exist" });

                    var dbUser = unit.Users.GetEager(userId.Id);
                    if (dbUser == null)
                        return BadRequest(new { message = $"User with id '{userId.Id}' did not exist" });

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

                    var res = new DTOAppointmentPrivate
                    {
                        Participants = new List<DTOSimpleUser>()
                    };

                    foreach (var participant in dbAppointment.Participants)
                    {
                        var simpleUser = new DTOSimpleUser();

                        Mapper.Map(participant.Calendar.User, simpleUser);

                        res.Participants.Add(simpleUser);
                    }

                    Mapper.Map(dbAppointment, res);

                    return CreatedAtAction("GetAppointment", new { appointmentId = res.Id }, res);

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
            var claims = User.Claims;
            var claimsUserId = claims.FirstOrDefault(x => x.Type == "id")?.Value;
            if (userId.ToString() != claimsUserId)
                return Unauthorized();

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
