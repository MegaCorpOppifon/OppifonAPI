using System;
using System.Collections.Generic;

namespace OppifonAPI.DTO.Calendar
{
    public class DTOAppointmentPrivate
    {
        public Guid Id { get; set; }
        public Guid OwnerId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public int MaxParticipants { get; set; }
        public ICollection<DTOSimpleUser> Participants { get; set; }
    }
}
