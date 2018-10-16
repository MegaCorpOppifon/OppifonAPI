using System;
using System.ComponentModel.DataAnnotations;

namespace DAL.Models
{
    public class Review
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string ReviewText { get; set; }
        public int Rating { get; set; }
        public bool Anonymity { get; set; }
    }
}
