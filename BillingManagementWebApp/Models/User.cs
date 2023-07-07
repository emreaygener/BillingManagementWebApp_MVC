using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace BillingManagementWebApp.Models
{
    [Index(nameof(TCNo), IsUnique = true)]
    [Index(nameof(Email), IsUnique = true)]
    [Index(nameof(PhoneNo), IsUnique = true)]
    public class User
    {
        [Key]
        public int Id { get; set; }
        public long TCNo { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        [Display(Name = "Phone Number")]
        public string PhoneNo { get; set; }
        public string? Plate { get; set; }
        public string Password { get; set; }
        [Display(Name = "Admin Authorization")]
        public bool isAdmin { get; set; }
        [Display(Name = "House Owner")]
        public bool isOwner { get; set; }

        
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpireDate { get; set; }
    }
}
