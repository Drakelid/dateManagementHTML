using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace dateManagementHTML.Models.Entities // ✅ Add this line
{
    public class Event
    {
        public int Id { get; set; }

        [Required]
        public string EventName { get; set; }

        public string? Description { get; set; }
        public string? Location { get; set; }

        [Required]
        public DateTime StartDateTime { get; set; }

        public DateTime? EndDateTime { get; set; }
        public bool IsAllDay { get; set; }

        [Required]
        public string Tag { get; set; }

        [ValidateNever]
        [Required]
        public string UserId { get; set; }

        public DateTime CreatedAt { get; set; }

        [ValidateNever]
        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }
    }
}
