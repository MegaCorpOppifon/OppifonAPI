using System;

namespace DAL.Models.ManyToMany
{
    public class ExpertTag
    {
        public Guid ExpertId { get; set; }
        public Expert Expert { get; set; }
        public Guid TagId { get; set; }
        public Tag Tag { get; set; }
    }
}
