using System.ComponentModel.DataAnnotations.Schema;
using System.Data.SqlTypes;

namespace BillingManagementWebApp.Models
{
    public class Invoice
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public DateTime DateCreated { get; set; }
        public double Cost { get; set; }
        public DateTime DateInvoicePaid { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }
        public User User { get; set; }

    }
}
