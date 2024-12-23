using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Transaction.Data;
using Transaction.Models;

namespace Transaction.Repositories
{
    public class TransactionTypeRepository : ITransactionTypeRepository
    {
        private readonly TransactionDbContext _context;
        public TransactionTypeRepository(TransactionDbContext context)
        {
            _context = context;
        }
        public async Task AddTransactionTypeAsync(TransactionType transactionType)
        {
            try
            {
                await _context.transaction_types.AddAsync(transactionType);
                await _context.SaveChangesAsync();
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
        public async Task<TransactionType> GetTransactionTypeByNameAsync(string name)
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
            catch (DbUpdateException)
            {
                throw;
            }
            catch (OperationCanceledException)
            {
                throw;
            }
        }
        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await _context.Database.BeginTransactionAsync();
        }
    }
}
