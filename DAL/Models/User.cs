using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using DAL.Models.ManyToMany;
using Microsoft.AspNetCore.Mvc;

namespace DAL.Models
{
    public class User : IdentityUser<Guid>
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        [Required]
        [Remote("doesUserNameExist", "Account", ErrorMessage = "Email already exists. Please enter a different email.")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [Display(Name = "Email")]
        public override string UserName { get; set; }
        public string City { get; set; }
        public DateTime Birthday { get; set; }
        public ICollection<UserTag> InterestTags { get; set; }
        public ICollection<UserFavorites> Favorites { get; set; }
        public string Gender { get; set; }
        public Calendar Calendar { get; set; }
        public bool IsExpert { get; set; }
        public string Image { get; set; }
    }
}
