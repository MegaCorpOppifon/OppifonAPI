using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DAL.Models.ManyToMany;

namespace DAL.Models
{
    public class Tag
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }

        public ICollection<ExpertTag> ExpertsTags { get; set; }
        public ICollection<MainFieldTag> ExpertsMainField { get; set; }
        public ICollection<UserTag> UsersTags { get; set; }
    }
}
