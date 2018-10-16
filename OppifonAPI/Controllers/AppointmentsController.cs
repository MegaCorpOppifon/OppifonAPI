using DAL.Factory;
using DAL.Models;
using DAL.Models.ManyToMany;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OppifonAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentsController : ControllerBase
    {
        private IFactory _factory;
        private readonly UserManager<User> _userManager;

        public AppointmentsController(UserManager<User> userManager, IFactory factory)
        {
            _factory = factory;
            _userManager = userManager;
        }

        // GET: api/Appointments
        [HttpGet]
        public IEnumerable<Appointment> GetAppointments()
        {
            using (var work = _factory.GetUOF())
            {
                return work.Appointments.GetAll();
            }
        }
        // GET: api/Appointments
        [HttpGet]
        public async Task<ICollection<CalendarAppointment>> GetUserAppointments()
        {
            var currentUser = await _userManager.FindByEmailAsync(_userManager.GetUserId(User));

            using (var work = _factory.GetUOF())
            {
                return work.Users.GetByEmail(currentUser.Email).Calendar.Appointments;
            }
        }

        // GET: api/Appointments/5
        [HttpGet("{id}")]
        public IActionResult GetAppointment([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            using (var work = _factory.GetUOF())
            {
                var appointment = work.Appointments.Get(id);

                if (appointment == null)
                {
                    return NotFound();
                }

                return Ok(appointment);
            }
        }

        // PUT: api/Appointments/5
        [HttpPut("{id}")]
        public IActionResult PutAppointment([FromRoute] Guid id, [FromBody] Appointment appointment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != appointment.Id)
            {
                return BadRequest();
            }

            using (var work = _factory.GetUOF())
            {
                var existingAppointment = work.Appointments.Get(id);

                existingAppointment.Time = appointment.Time;
                existingAppointment.Text = appointment.Text;
                existingAppointment.Participants = appointment.Participants;
                existingAppointment.Duration = appointment.Duration;
                existingAppointment.MaxParticipants = appointment.MaxParticipants;

                return Ok(existingAppointment);
            }            
        }

        // POST: api/Appointments
        [HttpPost]
        public IActionResult PostAppointment([FromBody] Appointment appointment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            using (var work = _factory.GetUOF())
            {
                work.Appointments.Add(appointment);

                return CreatedAtAction("GetAppointment", new { id = appointment.Id }, appointment);
            }
        }

        // DELETE: api/Appointments/5
        [HttpDelete("{id}")]
        public IActionResult DeleteAppointment([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            using (var work = _factory.GetUOF())
            {
                var appointment = work.Appointments.Get(id);
                if (appointment == null)
                {
                    return NotFound();
                }

                work.Appointments.Remove(appointment);

                return Ok(appointment);
            }
        }

        private bool AppointmentExists(Guid id)
        {
            using (var work = _factory.GetUOF())
            {
                return work.Appointments.GetAll().Any(e => e.Id == id);
            }
        }
    }
}