using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DAL.Models.ManyToMany;

namespace DAL.Models
{
    public class Calendar
    {
        [Key]
        public Guid Id { get; set; }
        public ICollection<CalendarAppointment> Appointments { get; set; }
        public ICollection<DayOff> DaysOff { get; set; }
        public ICollection<WorkDay> WorkDays { get; set; }
        public TimeSpan DefaultDuration { get; set; }

        public Guid UserId { get; set; }
        public User User { get; set; }
    }
}
