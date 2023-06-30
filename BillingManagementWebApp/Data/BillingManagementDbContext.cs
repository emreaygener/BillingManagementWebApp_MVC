using BillingManagementWebApp.Models;
using Microsoft.EntityFrameworkCore;

namespace BillingManagementWebApp.Data
{
    public class BillingManagementDbContext : DbContext, IBillingManagementDbContext
    {
        public BillingManagementDbContext(DbContextOptions<BillingManagementDbContext> options) : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Flat> Flats { get; set; }
        public DbSet<Due> Dues { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<Message> Messages { get; set; }
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
