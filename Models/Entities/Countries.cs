namespace dateManagementHTML.Models.Entities
{
    public class Country
    {
        public int Id { get; set; } // Internal DB ID
        public string CountryCode { get; set; } // ISO Code, e.g., "US"
        public string Name { get; set; } // Full name, e.g., "United States"
    }
}
