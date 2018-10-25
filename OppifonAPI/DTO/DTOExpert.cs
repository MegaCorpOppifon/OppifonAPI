using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.Models;

namespace OppifonAPI.DTO
{
    public class DTOExpert : DTOUser
    {
        public string ExpertCategory { get; set; }
        public string Description { get; set; }
        public ICollection<string> ExpertTags { get; set; }
        public ICollection<string> MainFields { get; set; }
        public ICollection<DTOReview> Reviews { get; set; }
    }
}
