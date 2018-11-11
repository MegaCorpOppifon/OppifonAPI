using System;

namespace OppifonAPI.DTO.Calendar
{
    public class DTOAppointment
    {
        public Guid Id { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public Guid OwnerId { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public int MaxParticipants { get; set; }
    }
}
