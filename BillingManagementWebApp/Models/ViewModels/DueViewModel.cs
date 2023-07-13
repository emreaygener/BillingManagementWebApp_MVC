using System.Data.SqlTypes;

namespace BillingManagementWebApp.Models.ViewModels
{
    public class DueViewModel
    {
        public int Id { get; set; }
        public DateTime? DateCreated { get; set; }
        public double Cost { get; set; }
        public DateTime? DateDuePaid { get; set; }

        public int UserId { get; set; }
        public string User { get; set; }
        public string UserTc { get; set; }
    }
}
