using BillingManagementWebApp.Data;
using BillingManagementWebApp.Models;
using Microsoft.EntityFrameworkCore;

namespace BillingManagementWebApp.Repositories
{
    public class DueRepository
    {
        private readonly IBillingManagementDbContext _context;

        public DueRepository(IBillingManagementDbContext context)
        {
            _context = context;
        }
        public async Task<List<Due>> GetAll()
        {
            return await _context.Dues.Include(x => x.User).OrderBy(x => x.Id).ToListAsync();
        }
        public async Task<List<Due>> GetAllForNonAdmin(string email)
        {
            return await _context.Dues.Include(x => x.User).Where(x => x.User.Email == email).OrderBy(x => x.Id).ToListAsync();
        }
        public async Task<Due> GetById(int id)
        {
            return await _context.Dues.Include(x => x.User).SingleOrDefaultAsync(x => x.Id == id);
        }
        public async Task<Due> GetByIdAsNoTracking(int id)
        {
            return await _context.Dues.AsNoTracking().Include(x => x.User).SingleOrDefaultAsync(x => x.Id == id);
        }
        public async Task Create(Due due)
        {
            _context.Dues.Add(due);
            await _context.SaveChangesAsync();
        }
        public async Task Update(Due due)
        {
            _context.Dues.Update(due);
            await _context.SaveChangesAsync();
        }
        public async Task Delete(Due due)
        {
            _context.Dues.Remove(due);
            await _context.SaveChangesAsync();
        }
    }
}
