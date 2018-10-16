using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Models.ManyToMany
{
    public class MainFieldTag
    {
        public Guid ExpertId { get; set; }
        public Expert Expert { get; set; }
        public Guid TagId { get; set; }
        public Tag Tag { get; set; }
    }
}
