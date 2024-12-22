using Identity.DTOs;
using Identity.Models;

namespace Identity.Services
{
    public interface IUserService
    {
        Task<User> GetByPhoneAsync(string phoneNumber);
        Task<User> GetByEmailAsync(string email);
        Task CreateUserAsync(SignUpDTO signUpDTO);
        Task CreateAdminAsync(SignUpDTO signUpDTO);
        Task<object> AuthorizeUserAsync(SignInDTO signInDTO);
        string HashPassword(string password);
        bool CheckPasswordAsync(User user, string password);
    }
}
