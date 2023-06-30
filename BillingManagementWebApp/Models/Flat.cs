using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace BillingManagementWebApp.Models
{
    public class Flat
    {
        public int Id { get; set; }
        public string BlockName { get; set; }
        public bool IsEmpthy { get; set; }
        public string Type { get; set; }
        public int FloorNo { get; set; }
        public int DoorNumber { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }
        public User User { get; set; } 
    }
}
