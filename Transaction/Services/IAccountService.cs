using Transaction.Models;

namespace Transaction.Services
{
    public interface IAccountService
    {
        Task CreateAccountAsync(Account account);
        Task FindByIdAsync(int userID);
        Task IncreaseBalanceAsync(int userID, decimal amount);
        Task DecreaseBalanceAsync(int userID, decimal amount);
    }
}
