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
        public Guid OwnerId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public ICollection<CalendarAppointment> Participants { get; set; }
        public int MaxParticipants { get; set; }
    }
}
