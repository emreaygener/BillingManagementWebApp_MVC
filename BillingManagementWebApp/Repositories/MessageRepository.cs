using BillingManagementWebApp.Data;
using BillingManagementWebApp.Models;
using Microsoft.EntityFrameworkCore;

namespace BillingManagementWebApp.Repositories
{
    public class MessageRepository
    {
        private readonly IBillingManagementDbContext _context;

        public MessageRepository(IBillingManagementDbContext context)
        {
            _context = context;
        }
        public async Task<List<Message>> GetAll()
        {
            return await _context.Messages.Include(x => x.Sender).Include(x=>x.Receiver).OrderBy(x => x.Id).ToListAsync();
        }
        public async Task<Message> GetById(int id)
        {
            return await _context.Messages.Include(x => x.Sender).Include(x => x.Receiver).SingleOrDefaultAsync(x => x.Id == id);
        }
        public async Task Create(Message message)
        {
            _context.Messages.Add(message);
            await _context.SaveChangesAsync();
        }
        public async Task Update(Message message)
        {
            _context.Messages.Update(message);
            await _context.SaveChangesAsync();
        }
        public async Task Delete(Message message)
        {
            _context.Messages.Remove(message);
            await _context.SaveChangesAsync();
        }
    }
}
