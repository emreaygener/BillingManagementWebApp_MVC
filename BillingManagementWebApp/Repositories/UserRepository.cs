using BillingManagementWebApp.Data;
using BillingManagementWebApp.Models;
using Microsoft.EntityFrameworkCore;

namespace BillingManagementWebApp.Repositories
{
    public class UserRepository
    {
        private readonly IBillingManagementDbContext _context;

        public UserRepository(IBillingManagementDbContext context)
        {
            _context = context;
        }
        public async Task<List<User>> GetAll()
        {
            return await _context.Users.OrderBy(x=>x.Id).ToListAsync();
        }
        public async Task<User> GetById(int id)
        {
            return await _context.Users.SingleOrDefaultAsync(x => x.Id == id);
        }
        public async Task<User> GetByTc(long Tc)
        {
            return await _context.Users.SingleOrDefaultAsync(x => x.TCNo == Tc);
        }
        public async Task Create(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }
        public async Task Update(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }
        public async Task Delete(User user)
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }
    }
}
