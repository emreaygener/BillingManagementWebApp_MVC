namespace BillingManagementWebApp.Models.ViewModels
{
    public class CreateUserViewModel
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
        public string Password { get; set; }
    }
}
