
using Microsoft.EntityFrameworkCore.Storage;
using Transaction.Models;

namespace Transaction.Repositories
{
    public interface ITransactionsRepository
    {
        Task AddTransactionAsync(Transactions transaction);
        Task GetAllTransactionsAsync(int userID);
        Task<IDbContextTransaction> BeginTransactionAsync();
    }
}
