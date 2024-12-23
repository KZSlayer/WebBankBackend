using Microsoft.EntityFrameworkCore;
using Transaction.DTOs;
using Transaction.Models;
using Transaction.Repositories;

namespace Transaction.Services.BaseServices
{
    public class TransactionsService : ITransactionsService
    {
        private readonly ITransactionsRepository _repository;
        public TransactionsService(ITransactionsRepository repository)
        {
            _repository = repository;
        }
        public async Task CreateTransactionAsync(long? fromAccountNumber, long? toAccountNumber, decimal amount, int transactionTypeID)
        {
            var _transaction = await _repository.BeginTransactionAsync();
            try
            {
                var transaction = new Transactions
                {
                    FromAccountNumber = fromAccountNumber,
                    ToAccountNumber = toAccountNumber,
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
                Console.WriteLine($"{ex.Message}");
                throw;
            }
            catch (OperationCanceledException)
            {
                throw;
            }
        }
        public async Task<List<TransactionDTO>> GetAccountTransactionsAsync(long accountNumber)
        {
            var accountTransactions = await _repository.SelectAllAccountTransactionsAsync(accountNumber);
            return accountTransactions;
        }
        public async Task<List<TransactionDTO>> GetTransactionsAsync()
        {
            var transactions = await _repository.SelectAllTransactionsAsync();
            return transactions;
        }
    }
}
