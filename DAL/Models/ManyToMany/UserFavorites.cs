using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Models.ManyToMany
{
    public class UserFavorites
    {
        public User User { get; set; }
        public Guid UserId { get; set; }
        public Expert Expert { get; set; }
        public Guid ExpertId { get; set; }
    }
}
