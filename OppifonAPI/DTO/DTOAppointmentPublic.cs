using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OppifonAPI.DTO
{
    public class DTOAppointmentPublic
    {
        public DateTime Time { get; set; }
        public TimeSpan Duration { get; set; }
    }
}
