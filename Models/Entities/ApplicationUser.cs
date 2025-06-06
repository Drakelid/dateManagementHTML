﻿using Microsoft.AspNetCore.Identity;

namespace dateManagementHTML.Models.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Role { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsActive { get; set; } = true;
        public string? PreferredCountryCode { get; set; }
    }
}