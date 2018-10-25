using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DAL.Models.ManyToMany;

namespace DAL.Models
{
    public class Appointment
    {
        [Key]
        public Guid Id { get; set; }
        public DateTime Time { get; set; }
        public string Text { get; set; }
        public ICollection<CalendarAppointment> Participants { get; set; }
        public TimeSpan Duration { get; set; }
        public int MaxParticipants { get; set; }
    }
}
