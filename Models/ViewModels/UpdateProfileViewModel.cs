using System.ComponentModel.DataAnnotations;

namespace dateManagementHTML.Models.ViewModels
{
    public class UpdateProfileViewModel
    {
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required, EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

}
