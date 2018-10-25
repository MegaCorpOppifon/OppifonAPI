using System;

namespace OppifonAPI.DTO
{
    public class DTOAppointment
    {
        public Guid Id { get; set; }
        public DateTime Time { get; set; }
        public TimeSpan Duration { get; set; }
        public string Name { get; set; }
        public Guid CreatorId { get; set; }
        public string Text { get; set; }
        public int MaxParticipants { get; set; }
    }
}
