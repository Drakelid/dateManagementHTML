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
        [Display(Name = "Country Code")]
        public string CountryCode { get; set; } // e.g. "NO", "DE"

        [Display(Name = "Custom Holiday")]
        public bool IsCustom { get; set; } = false;

        [Display(Name = "Active")]
        public bool IsActive { get; set; } = true;
    }
}