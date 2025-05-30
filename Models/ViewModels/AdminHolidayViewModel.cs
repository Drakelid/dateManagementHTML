using dateManagementHTML.Models.Entities;
using System.Collections.Generic;

namespace dateManagementHTML.Models.ViewModels
{
    public class AdminHolidayViewModel
    {
        public List<Holiday> Holidays { get; set; } = new List<Holiday>();
        public string? SelectedCountryCode { get; set; }

        // ⬇️ Add this property for the dropdown
        public List<Country> AvailableCountries { get; set; } = new List<Country>();
    }
}