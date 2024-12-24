using Identity.DTOs;
using Identity.Models;
using Microsoft.EntityFrameworkCore.Storage;

namespace Identity.Repositories
{
    public interface IUserRepository
    {
        Task AddUserAsync(User user);
        Task UpdateEmailAsync(User user);
        Task UpdatePhoneAsync(User user);
        Task UpdatePasswordAsync(User user);
        Task UpdatePassportDetailsAsync(User user);
        Task<List<UserDataDTO>> SelectAllUsersAsync();
        Task<UserDataDTO?> SelectUserByPhoneAsync(string phoneNumber);
        Task<User?> SelectByIdAsync(int userId);
        Task<User?> SelectByPhoneAsync(string phoneNumber);
        Task<User?> SelectByEmailAsync(string email);
        Task<IDbContextTransaction> BeginTransactionAsync();
    }
}
