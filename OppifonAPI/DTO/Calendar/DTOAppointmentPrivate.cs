using System;
using System.Collections.Generic;

namespace OppifonAPI.DTO
{
    public class DTOAppointmentPrivate
    {
        public Guid Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public int MaxParticipants { get; set; }
        public ICollection<DTOUser> Participants { get; set; }
    }
}
