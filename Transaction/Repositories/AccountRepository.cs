using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Security.Principal;
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
            try
            {
                await _dbContext.accounts.AddAsync(account);
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                throw;
            }
            catch (OperationCanceledException)
            {
                throw;
            }
        }
        public async Task<List<long>> GetAccountNumbersAsync()
        {
            return await _dbContext.accounts
                .Select(account => account.AccountNumber)
                .ToListAsync();
        }

        public async Task<Account> FindByUserIdAsync(int userID)
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
                throw;
            }
            catch (OperationCanceledException)
            {
                throw;
            }
        }
    }
}
