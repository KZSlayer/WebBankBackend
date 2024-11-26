using Microsoft.EntityFrameworkCore.Storage;
using Transaction.Models;

namespace Transaction.Repositories
{
    public interface IAccountRepository
    {
        Task AddAccountAsync(Account account);
        Task<Account> FindByIdAsync(int userID);
        Task<IDbContextTransaction> BeginTransactionAsync();
        Task SaveChangesAsync();
    }
}
