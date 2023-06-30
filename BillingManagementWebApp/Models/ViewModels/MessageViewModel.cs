namespace BillingManagementWebApp.Models.ViewModels
{
    public class MessageViewModel
    {
        public int Id { get; set; }
        public string Header { get; set; }
        public string Content { get; set; }
        public DateTime DateSent { get; set; }

        public int SenderId { get; set; }
        public string Sender { get; set; }
        public int ReceiverId { get; set; }
        public string Receiver { get; set; }
    }
}
