using Transaction.Models;

namespace Transaction.Services
{
    public interface IAccountService
    {
        Task CreateAccountAsync(Account account);
    }
}
