using System;

namespace DAL.Models.ManyToMany
{
    public class CalendarAppointment
    {
        public Guid CalendarId { get; set; }
        public Guid AppointmentId { get; set; }
        public Calendar Calendar { get; set; }
        public Appointment Appointment { get; set; }
    }
}
