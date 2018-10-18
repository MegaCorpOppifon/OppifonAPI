using System;
using System.Collections.Generic;
using DAL.Models;

namespace OppifonAPI.DTO
{
    // ReSharper disable once InconsistentNaming
    public class DTOUpdateUser
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string City { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime Birthday { get; set; }
        public ICollection<string> InterestTags { get; set; }
        public string Gender { get; set; }
        public bool IsExpert { get; set; }
    }
}
