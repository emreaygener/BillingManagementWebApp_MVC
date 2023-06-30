using System.Data.SqlTypes;

namespace BillingManagementWebApp.Models.ViewModels
{
    public class InvoiceViewModel
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public DateTime DateCreated { get; set; }
        public double Cost { get; set; }
        public DateTime DateInvoicePaid { get; set; }

        public int UserId { get; set; }
        public string User { get; set; }
    }
}
