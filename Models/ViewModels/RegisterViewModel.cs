using System.ComponentModel.DataAnnotations;

namespace dateManagementHTML.Models.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "You must agree to these terms if you wish to make an account.")]
        [Range(typeof(bool) ,"true", "true", ErrorMessage = "You must agree to these terms if you wish to make an account.")]
        public bool AgreeToTerms { get; set; }
    }
}
