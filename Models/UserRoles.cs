namespace NearU_Backend_Revised.Models
{
    /// <summary>
    /// Application role constants
    /// </summary>
    public static class UserRoles
    {
        public const string Student = "Student";
        public const string Rider = "Rider";
        public const string Business = "Business";
        public const string Admin = "Admin";

        /// <summary>
        /// Get all available roles
        /// </summary>
        public static readonly string[] AllRoles = 
        {
            Student,
            Rider,
            Business,
            Admin
        };

        /// <summary>
        /// Check if a role is valid
        /// </summary>
        public static bool IsValidRole(string role)
        {
            return AllRoles.Contains(role, StringComparer.OrdinalIgnoreCase);
        }
    }
}
