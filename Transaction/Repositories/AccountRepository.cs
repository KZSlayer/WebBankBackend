using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Diagnostics;
using System.Security.Principal;
using Transaction.Data;
using Transaction.Models;

namespace Transaction.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly TransactionDbContext _dbContext;
        private readonly ILogger<AccountRepository> _logger;
        public AccountRepository(TransactionDbContext dbContext, ILogger<AccountRepository> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task AddAccountAsync(Account account)
        {
            try
            {
                await _dbContext.accounts.AddAsync(account);
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError($"Ошибка при добавлении аккаунта в базу данных! Основная причина: {ex.InnerException?.Message}. Все детали: {ex}");
                throw;
            }
            catch (OperationCanceledException ex)
            {
                _logger.LogError($"Ошибка при добавлении аккаунта в базу данных! Основная причина: {ex.InnerException?.Message}. Все детали: {ex}");
                throw;
            }
        }
        public async Task<List<long>> GetAccountNumbersAsync()
        {
            return await _dbContext.accounts
                .Select(account => account.AccountNumber)
                .ToListAsync();
        }

        public async Task<Account?> FindByUserIdAsync(int userID)
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
            catch (DbUpdateException ex)
            {
                _logger.LogError($"Ошибка при сохранении данных аккаунта в базу данных! Основная причина: {ex.InnerException?.Message}. Все детали: {ex}");
                throw;
            }
            catch (OperationCanceledException ex)
            {
                _logger.LogError($"Ошибка при сохранении данных аккаунта в базу данных! Основная причина: {ex.InnerException?.Message}. Все детали: {ex}");
                throw;
            }
        }
    }
}
