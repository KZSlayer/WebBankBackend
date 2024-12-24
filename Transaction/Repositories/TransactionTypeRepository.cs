using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Transaction.Data;
using Transaction.Models;

namespace Transaction.Repositories
{
    public class TransactionTypeRepository : ITransactionTypeRepository
    {
        private readonly TransactionDbContext _context;
        private readonly ILogger<TransactionTypeRepository> _logger;
        public TransactionTypeRepository(TransactionDbContext context, ILogger<TransactionTypeRepository> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task AddTransactionTypeAsync(TransactionType transactionType)
        {
            try
            {
                await _context.transaction_types.AddAsync(transactionType);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError($"Ошибка при добавлении типа транзакции в базу данных! Основная причина: {ex.InnerException?.Message}. Все детали: {ex}");
                throw;
            }
            catch (OperationCanceledException ex)
            {
                _logger.LogError($"Ошибка при добавлении типа транзакции в базу данных! Основная причина: {ex.InnerException?.Message}. Все детали: {ex}");
                throw;
            }
        }
        public async Task<TransactionType?> GetTransactionTypeByNameAsync(string name)
        {
            return await _context.transaction_types.FirstOrDefaultAsync(t => t.Name == name);
        }
        public async Task<bool> CheckIfTransactionTypeExistAsync(string name)
        {
            return await _context.transaction_types.AnyAsync(t => t.Name == name);
        }
        public async Task SaveChangesAsync()
        {
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError($"Ошибка при сохранении типа транзакции в базу данных! Основная причина: {ex.InnerException?.Message}. Все детали: {ex}");
                throw;
            }
            catch (OperationCanceledException ex)
            {
                _logger.LogError($"Ошибка при сохранении типа транзакции в базу данных! Основная причина: {ex.InnerException?.Message}. Все детали: {ex}");
                throw;
            }
        }
        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await _context.Database.BeginTransactionAsync();
        }
    }
}
