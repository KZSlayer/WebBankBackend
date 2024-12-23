
using Microsoft.EntityFrameworkCore.Storage;
using Transaction.DTOs;
using Transaction.Models;

namespace Transaction.Repositories
{
    public interface ITransactionsRepository
    {
        Task AddTransactionAsync(Transactions transaction);
        Task<List<TransactionDTO>> SelectAllAccountTransactionsAsync(long accountNumber);
        Task<List<TransactionDTO>> SelectAllTransactionsAsync();
        Task<IDbContextTransaction> BeginTransactionAsync();
    }
}
