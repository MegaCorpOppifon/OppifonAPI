using System;
using System.Collections.Generic;
using DAL.Models;

namespace OppifonAPI.DTO
{
    // ReSharper disable once InconsistentNaming
    public class DTORegisterUser
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string City { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime Birthday { get; set; }
        public ICollection<string> InterestTags { get; set; }
        public string Gender { get; set; }
        public bool IsExpert { get; set; }
        public string ExpertCategory { get; set; }
        public ICollection<string> ExpertTags { get; set; }
        public string Description { get; set; }
        public ICollection<string> MainFields { get; set; }
    }
}
