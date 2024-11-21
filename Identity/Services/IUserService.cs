using Identity.DTOs;
using Identity.Models;

namespace Identity.Services
{
    public interface IUserService
    {
        Task<User> GetByPhoneAsync(string phoneNumber);
        Task<User> GetByEmailAsync(string email);
        Task CreateUserAsync(User user);
        User ConvertToUser(SignUpDTO signUpDTO);
        string HashPassword(string password);
    }
}
