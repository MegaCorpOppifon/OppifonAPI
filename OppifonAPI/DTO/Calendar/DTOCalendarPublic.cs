using System;
using System.Collections.Generic;
using DAL.Models;

namespace OppifonAPI.DTO.Calendar
{
    public class DTOCalendarPublic
    {
        public Guid Id { get; set; }
        public ICollection<DTOAppointmentPublic> Appointments { get; set; }
        public ICollection<DayOff> DaysOff { get; set; }
        public ICollection<WorkDay> WorkDays { get; set; }
        public TimeSpan DefaultDuration { get; set; }
    }
}
