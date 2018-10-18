using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.Models;
using DAL.Models.ManyToMany;

namespace OppifonAPI.DTO
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
