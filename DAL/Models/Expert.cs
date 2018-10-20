using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DAL.Models.ManyToMany;

namespace DAL.Models
{
    public class Expert : User
    {
        //[Key]
        //public Guid Id { get; set; }
        public Category ExpertCategory { get; set; }
        public string Description { get; set; }
        public ICollection<ExpertTag> ExpertTags { get; set; }
        public ICollection<MainFieldTag> MainFields { get; set; }
        public ICollection<Review> Reviews { get; set; }
    }
}
