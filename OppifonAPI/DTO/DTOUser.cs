using System;
using System.Collections.Generic;

namespace OppifonAPI.DTO
{
    public class DTOUser
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string City { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime Birthday { get; set; }
        public ICollection<string> InterestTags { get; set; }
        public string Gender { get; set; }
        public bool IsExpert { get; set; }
    }
}
