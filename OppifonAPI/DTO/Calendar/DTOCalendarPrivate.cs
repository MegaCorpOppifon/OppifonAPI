using System;
using System.Collections.Generic;
using DAL.Models;

namespace OppifonAPI.DTO
{
    public class DTOCalendarPrivate
    {
        public Guid Id { get; set; }
        public ICollection<DTOAppointmentPrivate> Appointments { get; set; }
        public ICollection<DayOff> DaysOff { get; set; }
        public ICollection<WorkDay> WorkDays { get; set; }
        public TimeSpan DefaultDuration { get; set; }
    }
}
