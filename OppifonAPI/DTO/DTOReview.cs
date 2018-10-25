using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OppifonAPI.DTO
{
    public class DTOReview
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string ReviewText { get; set; }
        public int Rating { get; set; }
        public bool Anonymity { get; set; }
    }
}
