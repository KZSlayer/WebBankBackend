using Microsoft.EntityFrameworkCore.Storage;
using Transaction.Models;

namespace Transaction.Repositories
{
    public interface IAccountRepository
    {
        Task AddAccountAsync(Account account);
        Task<IDbContextTransaction> BeginTransactionAsync();
    }
}
