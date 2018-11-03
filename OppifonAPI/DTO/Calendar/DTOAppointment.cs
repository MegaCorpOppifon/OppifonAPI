using System;

namespace OppifonAPI.DTO
{
    public class DTOAppointment
    {
        public Guid Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Name { get; set; }
        public Guid CreatorId { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public int MaxParticipants { get; set; }
    }
}
