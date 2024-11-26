using Microsoft.EntityFrameworkCore;
using Transaction.Models;
using Transaction.Repositories;

namespace Transaction.Services
{
    public class TransactionsService : ITransactionsService
    {
        private readonly ITransactionsRepository _repository;
        public TransactionsService(ITransactionsRepository repository)
        {
            _repository = repository;
        }
        public async Task CreateTransactionAsync(int? fromAccountID, int? toAccountID, decimal amount, int transactionTypeID)
        {
            var _transaction = await _repository.BeginTransactionAsync();
            try
            {
                var transaction = new Transactions
                {
                    FromAccountUserId = fromAccountID,
                    ToAccountUserId = toAccountID,
                    Amount = amount,
                    TransactionTypeId = transactionTypeID,
                    Status = "Успешно",
                    Timestamp = DateTime.UtcNow,
                };
                await _repository.AddTransactionAsync(transaction);
                await _transaction.CommitAsync();
            }
            catch (DbUpdateException ex)
            {
                await _transaction.RollbackAsync();
                Console.WriteLine($"Ошибка в ser AddTransactionAsync \n{ex.Message}");
                throw new DbUpdateException();
            }
        }
    }
}
