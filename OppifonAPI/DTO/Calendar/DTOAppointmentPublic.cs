using System;

namespace OppifonAPI.DTO
{
    public class DTOAppointmentPublic
    {
        public Guid Id { get; set; }
        public DateTime Time { get; set; }
        public TimeSpan Duration { get; set; }
    }
}
