﻿using Microsoft.AspNetCore.Identity;

namespace dateManagementHTML.Models.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string Role { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsActive { get; set; } = true;
    }
}