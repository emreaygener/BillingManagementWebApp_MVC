using BillingManagementWebApp.Data;
using BillingManagementWebApp.Models;
using Microsoft.EntityFrameworkCore;

namespace BillingManagementWebApp.Repositories
{
    public class InvoiceRepository
    {
        private readonly IBillingManagementDbContext _context;

        public InvoiceRepository(IBillingManagementDbContext context)
        {
            _context = context;
        }
        public async Task<List<Invoice>> GetAll()
        {
            return await _context.Invoices.Include(x => x.User).OrderBy(x => x.Id).ToListAsync();
        }
        public async Task<Invoice> GetById(int id)
        {
            return await _context.Invoices.Include(x => x.User).SingleOrDefaultAsync(x => x.Id == id);
        }
        public async Task Create(Invoice invoice)
        {
            _context.Invoices.Add(invoice);
            await _context.SaveChangesAsync();
        }
        public async Task Update(Invoice invoice)
        {
            _context.Invoices.Update(invoice);
            await _context.SaveChangesAsync();
        }
        public async Task Delete(Invoice invoice)
        {
            _context.Invoices.Remove(invoice);
            await _context.SaveChangesAsync();
        }
    }
}
