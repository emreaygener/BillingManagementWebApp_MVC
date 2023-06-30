using BillingManagementWebApp.Data;
using BillingManagementWebApp.Models;
using Microsoft.EntityFrameworkCore;

namespace BillingManagementWebApp.Repositories
{
    public class FlatRepository
    {
        private readonly IBillingManagementDbContext _context;

        public FlatRepository(IBillingManagementDbContext context)
        {
            _context = context;
        }
        public async Task<List<Flat>> GetAll()
        {
            return await _context.Flats.Include(x => x.User).OrderBy(x => x.Id).ToListAsync();
        }
        public async Task<Flat> GetById(int id)
        {
            return await _context.Flats.Include(x => x.User).SingleOrDefaultAsync(x => x.Id == id);
        }
        public async Task Create(Flat flat)
        {
            _context.Flats.Add(flat);
            await _context.SaveChangesAsync();
        }
        public async Task Update(Flat flat)
        {
            _context.Flats.Update(flat);
            await _context.SaveChangesAsync();
        }
        public async Task Delete(Flat flat)
        {
            _context.Flats.Remove(flat);
            await _context.SaveChangesAsync();
        }
    }
}
