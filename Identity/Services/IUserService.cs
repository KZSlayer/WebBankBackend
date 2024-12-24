using Identity.DTOs;
using Identity.Models;

namespace Identity.Services
{
    public interface IUserService
    {
        Task CreateUserAsync(SignUpDTO signUpDTO);
        Task CreateAdminAsync(SignUpDTO signUpDTO);
        Task EditEmailAsync(ChangeEmailDTO changeEmailDTO);
        Task EditPhoneAsync(ChangePhoneDTO changePhoneDTO);
        Task EditPasswordAsync(ChangePasswordDTO changePasswordDTO);
        Task EditPassportDetailsAsync(ChangePassportDetailsDTO changePassportDetailsDTO);
        Task<List<UserDataDTO>> GetAllUsers();
        Task<UserDataDTO> GetUserByPhoneAsync(string phoneNumber);
        Task<User?> GetByPhoneAsync(string phoneNumber);
        Task<User?> GetByEmailAsync(string email);
        Task<object> AuthorizeUserAsync(SignInDTO signInDTO);
        string HashPassword(string password);
        bool CheckPasswordAsync(User user, string password);
    }
}
