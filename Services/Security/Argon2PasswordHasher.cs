using Microsoft.AspNetCore.Identity;
using Isopoh.Cryptography.Argon2;

namespace dateManagementHTML.Services.Security
{
    public class Argon2PasswordHasher<TUser> : IPasswordHasher<TUser> where TUser : class
    {
        public string HashPassword(TUser user, string password)
        {
            return Argon2.Hash(password);
        }

        public PasswordVerificationResult VerifyHashedPassword(TUser user, string hashedPassword, string providedPassword)
        {
            bool isValid = Argon2.Verify(hashedPassword, providedPassword);
            return isValid ? PasswordVerificationResult.Success : PasswordVerificationResult.Failed;
        }
    }
}
