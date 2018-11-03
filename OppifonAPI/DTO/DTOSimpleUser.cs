using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OppifonAPI.DTO
{
    public class DTOSimpleUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public Guid Id { get; set; }
    }
}
