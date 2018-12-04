using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OppifonAPI.DTO
{
    public class DTOPublicExpert
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string City { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime Birthday { get; set; }
        public string Gender { get; set; }
        public bool IsExpert { get; set; }
        public string ExpertCategory { get; set; }
        public string Description { get; set; }
        public ICollection<string> ExpertTags { get; set; }
        public ICollection<string> MainFields { get; set; }
        public ICollection<DTOReview> Reviews { get; set; }
        public string Image { get; set; }
    }
}
