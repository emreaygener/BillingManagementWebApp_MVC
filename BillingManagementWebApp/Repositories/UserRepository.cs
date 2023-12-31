﻿using BillingManagementWebApp.Auth;
using BillingManagementWebApp.Auth.Model;
using BillingManagementWebApp.Data;
using BillingManagementWebApp.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace BillingManagementWebApp.Repositories
{
    public class UserRepository
    {
        private readonly IBillingManagementDbContext _context;
        private readonly IConfiguration _configuration;
        public UserRepository(IBillingManagementDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        public async Task<List<User>> GetAll()
        {
            return await _context.Users.OrderBy(x => x.Id).ToListAsync();
        }
        public async Task<User> GetById(int id)
        {
            return await _context.Users.SingleOrDefaultAsync(x => x.Id == id);
        }
        public async Task<User> GetByIdNoTracking(int id)
        {
            return await _context.Users.AsNoTracking().SingleOrDefaultAsync(x => x.Id == id);
        }
        public async Task<User> GetByTc(long Tc)
        {
            return await _context.Users.SingleOrDefaultAsync(x => x.TCNo == Tc);
        }
        public async Task<User> GetByCredentials(string username, string password)
        {
            return await _context.Users.SingleOrDefaultAsync(x => (x.Email == username || x.PhoneNo == username || x.TCNo == Convert.ToInt64(username)) && x.Password == password);
        }
        public async Task<User> GetByCredentials(string username)
        {
            var fromEmail = await _context.Users.SingleOrDefaultAsync(x => x.Email == username);
            if (fromEmail != null)
            {
                return fromEmail;
            }
            var fromPhone = await _context.Users.SingleOrDefaultAsync(x => x.PhoneNo == username);
            if (fromPhone != null)
            {
                return fromPhone;
            }
            var fromTc = await _context.Users.SingleOrDefaultAsync(x => x.TCNo == Convert.ToInt64(username));
            return fromTc;
        }
        public async Task<User> GetByEmail(string email)
        {
            return await _context.Users.SingleOrDefaultAsync(x => x.Email == email);
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
        public async Task<Token> CreateToken(User user)
        {
            TokenHandler handler = new TokenHandler(_configuration);
            Token token = handler.CreateAccessToken(user);
            user.RefreshToken = token.RefreshToken;
            user.RefreshTokenExpireDate = token.ExpirationDate.AddMinutes(5);
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return token;
        }
        public async Task RefreshToken(User user)
        {
            TokenHandler handler = new TokenHandler(_configuration);
            Token token = handler.CreateAccessToken(user);
            user.RefreshToken = token.RefreshToken;
            user.RefreshTokenExpireDate = token.ExpirationDate.AddMinutes(5);
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }
    }
}
