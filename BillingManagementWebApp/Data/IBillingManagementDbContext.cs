using BillingManagementWebApp.Models;
using Microsoft.EntityFrameworkCore;

namespace BillingManagementWebApp.Data
{
    public interface IBillingManagementDbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Flat> Flats { get; set; }
        public DbSet<Due> Dues { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<Message> Messages { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
