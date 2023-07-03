using Microsoft.EntityFrameworkCore;

namespace BillingManagementWebApp.Models
{
    [Index(nameof(TCNo), IsUnique = true)]
    [Index(nameof(Email), IsUnique = true)]
    [Index(nameof(PhoneNo), IsUnique = true)]
    public class User
    {
        public int Id { get; set; }
        public long TCNo { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string PhoneNo { get; set; }
        public string? Plate { get; set; }
        public string Password { get; set; } = new Guid().ToString();
        public bool isAdmin { get; set; } = false;
        public bool isOwner { get; set; }

        
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpireDate { get; set; }
    }
}
