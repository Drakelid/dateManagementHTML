using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace dateManagementHTML.Models.Entities // ✅ Add this line
{
    public class Event
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Event Name")]
        public string EventName { get; set; }

        [Display(Name = "Description")]
        public string? Description { get; set; }

        [Display(Name = "Location")]
        public string? Location { get; set; }

        [Required]
        [Display(Name = "From")]
        public DateTime StartDateTime { get; set; }

        [Display(Name = "To")]
        public DateTime? EndDateTime { get; set; }
        
        [Display(Name = "All day")]
        public bool IsAllDay { get; set; }

        [Required]
        [Display(Name = "Tag")]
        public string Tag { get; set; }

        [ValidateNever]
        [Required]
        public string UserId { get; set; }

        public DateTime CreatedAt { get; set; }

        [ValidateNever]
        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }

        [Display(Name = "Completed")]
        public bool IsCompleted { get; set; } = false;

    }
}
