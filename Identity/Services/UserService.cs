using Identity.DTOs;
using Identity.Models;
using Identity.Repositories;
using Identity.Repositories.Exceptions;
using Identity.Services.Exceptions;
using Microsoft.AspNetCore.Identity;

namespace Identity.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;
        private readonly PasswordHasher<User> _passwordHasher;
        public UserService(IUserRepository repository)
        {
            _repository = repository;
            _passwordHasher = new PasswordHasher<User>();
        }

        public User ConvertToUser(SignUpDTO signUpDTO)
        {
            return new User
            {
                FirstName = signUpDTO.FirstName,
                LastName = signUpDTO.LastName,
                DateOfBirth = signUpDTO.DateOfBirth,
                Email = signUpDTO.Email,
                PhoneNumber = signUpDTO.PhoneNumber,
                Role = "user"
            };
        }

        public async Task CreateUserAsync(User user)
        {
            var _transation = await _repository.BeginTransactionAsync();
            try
            {
                await _repository.AddUserAsync(user);
                await _transation.CommitAsync();
            }
            catch (UserSaveFailedException)
            {
                await _transation.RollbackAsync();
                throw new UserCreateFailedException("Не удалось сохранить пользователя в базе данных.");
            }
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            return await _repository.FindByEmailAsync(email);
        }

        public async Task<User> GetByPhoneAsync(string phoneNumber)
        {
            return await _repository.FindByPhoneAsync(phoneNumber);
        }

        public string HashPassword(string password)
        {
            var user = new User();
            return _passwordHasher.HashPassword(user,password);
        }
        public bool CheckPasswordAsync(User user, string password)
        {
            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);
            return result == PasswordVerificationResult.Success;
        }
    }
}
