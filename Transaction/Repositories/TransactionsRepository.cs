using Microsoft.EntityFrameworkCore.Storage;
using Transaction.Data;
using Transaction.Models;

namespace Transaction.Repositories
{
    public class TransactionsRepository : ITransactionsRepository
    {
        private readonly TransactionDbContext _context;
        public TransactionsRepository(TransactionDbContext context)
        {
            _context = context;
        }
        public async Task AddTransactionAsync(Transactions transaction)
        {
            await _context.transactions.AddAsync(transaction);
            await _context.SaveChangesAsync();
        }
        public Task GetAllTransactionsAsync(int userID)
        {
            var transactions = _context.transactions
                .Where(t => t.FromAccountId == userID || t.ToAccountId == userID)
                .Select(t => new
                {
                    t.Id,
                    t.Amount,
                    t.Status,
                    t.Timestamp,
                    FromAccount = t.FromAccountId,
                    ToAccount = t.ToAccountId,
                    TransactionType = t.TransactionType.Name
                })
                .ToList();
            return Task.FromResult(transactions);
        }
        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await _context.Database.BeginTransactionAsync();
        }
    }
}
