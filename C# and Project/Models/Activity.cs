using System;
using System.ComponentModel.DataAnnotations;

namespace C__and_Project.Models
{
    public class Activity
    {
        public int ActivityID { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public TimeSpan StartTime { get; set; }

        [Required]
        public int Duration { get; set; }
    }
}
