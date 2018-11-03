using System;

namespace OppifonAPI.DTO.Calendar
{
    public class DTOAppointmentPublic
    {
        public Guid Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}
