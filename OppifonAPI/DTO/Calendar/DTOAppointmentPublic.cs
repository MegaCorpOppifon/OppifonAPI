using System;

namespace OppifonAPI.DTO
{
    public class DTOAppointmentPublic
    {
        public Guid Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}
