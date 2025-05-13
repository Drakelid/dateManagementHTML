using System.ComponentModel.DataAnnotations;

namespace dateManagementHTML.Models.Entities
{
    public class Appointment
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }
    }
}
