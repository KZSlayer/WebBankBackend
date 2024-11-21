using Identity.Models;
using Microsoft.EntityFrameworkCore.Storage;

namespace Identity.Repositories
{
    public interface IUserRepository
    {
        Task<User> FindByPhoneAsync(string phoneNumber);
        Task<User> FindByEmailAsync(string email);
        Task AddUserAsync(User user);
        Task<IDbContextTransaction> BeginTransactionAsync();
    }
}
