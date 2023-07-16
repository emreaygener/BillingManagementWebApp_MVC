using System.ComponentModel.DataAnnotations;

namespace BillingManagementWebApp.Models.ViewModels
{
    public class CardViewModel
    {
        public string CardNumber { get; set; }
        public string CardholderName { get; set; }
        public int ExpireMonth { get; set; }
        public int ExpireYear { get; set; }
        [Range(100, 999, ErrorMessage = "Invalid CVV Number")]
        public int CVVCode { get; set; }
        public int PaymentId { get; set; }
        public double AmountOfPayment { get; set; }
    }
}
