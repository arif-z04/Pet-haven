namespace PetHaven.Api.Helpers;

public static class PasswordHasher
{
    public static string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.EnhancedHashPassword(password, 11);
    }

    public static bool VerifyPassword(string password, string passwordHash)
    {
        try
        {
            return BCrypt.Net.BCrypt.EnhancedVerify(password, passwordHash);
        }
        catch
        {
            // Fallback for standard bcrypt hashes seeded in the database
            return BCrypt.Net.BCrypt.Verify(password, passwordHash);
        }
    }
}
