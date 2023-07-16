using System.ComponentModel.DataAnnotations;

namespace TransactionServiceWebAPI.Models
{
    public class CardViewModel
    {
        [Required]
        public string CardNumber { get; set; }
        [Required]
        public string CardholderName { get; set; }
        [Required]
        [Range(1, 12, ErrorMessage = "Invalid Month Number")]
        public int ExpireMonth { get; set; }
        [Required]
        [Range(2023, 2030, ErrorMessage = "Invalid Year Number")]
        public int ExpireYear { get; set; }
        [Required]
        [Range(100, 999, ErrorMessage = "Invalid CVV Number")]
        public int CVVCode { get; set; }
        [Required]
        public double AmountOfPayment { get; set; }
    }
}
