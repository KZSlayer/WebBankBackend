using Transaction.DTOs;
using Transaction.Models;

namespace Transaction.Services.BaseServices
{
    public interface ITransactionsService
    {
        Task CreateTransactionAsync(long? fromAccountNumber, long? toAccountNumber, decimal amount, int transactionTypeID);
        Task<List<TransactionDTO>> GetAccountTransactionsAsync(long accountNumber);
        Task<List<TransactionDTO>> GetTransactionsAsync();
    }
}
