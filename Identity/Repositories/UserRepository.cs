using Identity.Data;
using Identity.Models;
using Identity.Repositories.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Identity.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IdentityDbContext _context;
        public UserRepository(IdentityDbContext context)
        {
            _context = context;
        }
        public async Task AddUserAsync(User user)
        {
            try
            {
                await _context.users.AddAsync(user);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                throw new UserSaveFailedException("Не удалось сохранить пользователя в базе данных.");
            }
        }

        public async Task<User> FindByEmailAsync(string email)
        {
            return await _context.users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User> FindByPhoneAsync(string phoneNumber)
        {
            return await _context.users.FirstOrDefaultAsync(u => u.PhoneNumber == phoneNumber);
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await _context.Database.BeginTransactionAsync();
        }

    }
}
