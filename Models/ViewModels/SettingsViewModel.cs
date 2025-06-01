using Microsoft.AspNetCore.Mvc.Rendering;

namespace dateManagementHTML.Models.ViewModels
{
    public class SettingsViewModel
    {
        public UpdateProfileViewModel Profile { get; set; }
        public UpdatePasswordViewModel Password { get; set; }

        public string PreferredCountryCode { get; set; }
        public List<SelectListItem> CountrySelectList { get; set; }
    }
}