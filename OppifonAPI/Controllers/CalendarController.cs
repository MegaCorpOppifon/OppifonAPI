﻿using System;
using System.Collections.Generic;
using System.Linq;
using DAL.Factory;
using DAL.Models;
using DAL.Models.ManyToMany;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OppifonAPI.DTO;

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

        #region user

        [HttpGet("user/{id}")]
        public IActionResult GetCalendarUser(Guid id)
        {
            var claims = User.Claims;
            var isExpert = claims.FirstOrDefault(x => x.Type == "isExpert")?.Value;
            if (isExpert != "True")
                return Unauthorized();

            using (var unit = _factory.GetUOF())
            {
                try
                {
                    var dbCalendar = unit.Calendars.GetEagerByUserId(id);
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
                            dtoAppointment.Participants.Add(new DTOUser
                            {
                                Id = participant.Id,
                                InterestTags = new List<string>(),
                                IsExpert = participant.IsExpert,
                                Email = participant.Email,
                                City = participant.City,
                                Gender = participant.Gender,
                                Birthday = participant.Birthday,
                                LastName = participant.LastName,
                                FirstName = participant.FirstName,
                                PhoneNumber = participant.PhoneNumber
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

        [HttpPost("{id}/appointment")]
        public IActionResult AddAppointmentUser([FromBody] DTOAppointment dtoAppointment, Guid id)
        {
            using (var unit = _factory.GetUOF())
            {
                try
                {
                    var dbExpert = unit.Experts.GetEager(id);

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

        #endregion

        #region expert

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

        #endregion

    }
}