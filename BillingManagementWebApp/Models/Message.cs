using System.ComponentModel.DataAnnotations.Schema;

namespace BillingManagementWebApp.Models
{
    public class Message
    {
        public int Id { get; set; }
        public string Header { get; set; }
        public string Content { get; set; }
        public DateTime DateSent { get; set; }

        [ForeignKey("Sender")]
        public int SenderId { get; set; }
        public User Sender { get; set; }

        [ForeignKey("Receiver")]
        public int ReceiverId { get; set; }
        public User Receiver { get; set; }
    }
}
