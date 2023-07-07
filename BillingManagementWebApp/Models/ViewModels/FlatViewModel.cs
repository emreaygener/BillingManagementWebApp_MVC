namespace BillingManagementWebApp.Models.ViewModels
{
    public class FlatViewModel
    {
        public int Id { get; set; }
        public string BlockName { get; set; }
        public bool IsEmpthy { get; set; }
        public string Type { get; set; }
        public int FloorNo { get; set; }
        public int DoorNumber { get; set; }
        public int UserId { get; set; }
        public string User { get; set; }
        public string UserTc { get; set; }
    }
}
