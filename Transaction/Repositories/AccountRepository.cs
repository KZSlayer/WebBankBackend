using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Transaction.Data;
using Transaction.Models;

namespace Transaction.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly TransactionDbContext _dbContext;
        public AccountRepository(TransactionDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAccountAsync(Account account)
        {
            await _dbContext.accounts.AddAsync(account);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Account> FindByIdAsync(int userID)
        {
            return await _dbContext.accounts.FirstOrDefaultAsync(ac => ac.UserId == userID);
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await _dbContext.Database.BeginTransactionAsync();
        }

        public async Task SaveChangesAsync()
        {
            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                throw new DbUpdateException("Данные не сохранились");
            }
            
        }
    }
}
