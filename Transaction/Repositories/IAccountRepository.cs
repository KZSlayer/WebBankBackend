using Microsoft.EntityFrameworkCore.Storage;
using Transaction.Models;

namespace Transaction.Repositories
{
    public interface IAccountRepository
    {
        Task AddAccountAsync(Account account);
        Task<Account?> FindByUserIdAsync(int userID);
        Task<List<long>> GetAccountNumbersAsync();
        Task<IDbContextTransaction> BeginTransactionAsync();
        Task SaveChangesAsync();
    }
}
