using System.ComponentModel.DataAnnotations;

namespace dateManagementHTML.Models.Entities
{
    public class Country
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }    // e.g. "Norway"

        [Required]
        public string IsoCode { get; set; } // e.g. "NO"
    }
}