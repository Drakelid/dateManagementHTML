using System;
using System.ComponentModel.DataAnnotations;

namespace dateManagementHTML.Models.Entities
{
    public class Holiday
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public string CountryCode { get; set; } // e.g. "NO", "DE"
    }
}