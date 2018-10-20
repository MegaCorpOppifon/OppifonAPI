using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OppifonAPI.DTO
{
    public class DTOAppointment
    {
        public DateTime Time { get; set; }
        public TimeSpan Duration { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Text { get; set; }
        public int MaxParticipants { get; set; }
    }
}
