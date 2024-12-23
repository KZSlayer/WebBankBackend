using Transaction.Models;

namespace Transaction.Services.BaseServices
{
    public interface IAccountService
    {
        Task CreateAccountAsync(Account account);
        Task<long> GenerateAccountNumber();
        Task<Account> FindByUserIdAsync(int userID);
        Task IncreaseBalanceAsync(Account account, decimal amount);
        Task DecreaseBalanceAsync(Account account, decimal amount);
    }
}
