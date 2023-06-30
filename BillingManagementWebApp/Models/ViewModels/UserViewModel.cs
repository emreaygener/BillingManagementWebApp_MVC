namespace BillingManagementWebApp.Models.ViewModels
{
    public class UserViewModel
    {
        public int Id { get; set; }
        public long TCNo { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string PhoneNo { get; set; }
        public string? Plate { get; set; }
        public bool isAdmin { get; set; } = false;
        public bool isOwner { get; set; }
    }
}
