using System;
using System.Collections.Generic;

namespace OppifonAPI.DTO
{
    public class DTOAppointmentPrivate
    {
        public DateTime Time { get; set; }
        public TimeSpan Duration { get; set; }
        public ICollection<DTOUser> Participants { get; set; }
    }
}
