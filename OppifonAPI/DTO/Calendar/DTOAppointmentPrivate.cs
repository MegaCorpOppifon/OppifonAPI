using System;
using System.Collections.Generic;

namespace OppifonAPI.DTO
{
    public class DTOAppointmentPrivate
    {
        public Guid Id { get; set; }
        public DateTime Time { get; set; }
        public TimeSpan Duration { get; set; }
        public ICollection<DTOUser> Participants { get; set; }
    }
}
