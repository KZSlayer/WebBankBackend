using Microsoft.EntityFrameworkCore.Storage;
using Transaction.Models;

namespace Transaction.Repositories
{
    public interface ITransactionTypeRepository
    {
        Task AddTransactionTypeAsync(TransactionType transactionType);
        Task<TransactionType?> GetTransactionTypeByNameAsync(string name);
        Task<bool> CheckIfTransactionTypeExistAsync(string name);
        Task SaveChangesAsync();
        Task<IDbContextTransaction> BeginTransactionAsync();
    }
}
