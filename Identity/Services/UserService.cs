using Identity.DTOs;
using Identity.Messaging;
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
        private readonly ITokenService _tokenService;
        private readonly IKafkaProducerService _kafkaProducerService;
        private readonly PasswordHasher<User> _passwordHasher;
        public UserService(IUserRepository repository, ITokenService tokenService, IKafkaProducerService kafkaProducerService)
        {
            _repository = repository;
            _passwordHasher = new PasswordHasher<User>();
            _tokenService = tokenService;
            _kafkaProducerService = kafkaProducerService;
        }

        public async Task CreateUserAsync(SignUpDTO signUpDTO)
        {
            if (signUpDTO.DateOfBirth > DateTime.Now)
            {
                throw new FutureDateOfBirthException();
            }
            if (signUpDTO.DateOfBirth > DateTime.Now.AddYears(-18))
            {
                throw new UnderageRegistrationException();
            }
            var user_by_phone = await GetByPhoneAsync(signUpDTO.PhoneNumber);
            if (user_by_phone != null)
            {
                throw new UserPhoneAlreadyExist();
            }
            var user_by_email = await GetByEmailAsync(signUpDTO.Email);
            if (user_by_email != null)
            {
                throw new UserEmailAlreadyExist();
            }
            var _user = new User
            {
                FirstName = signUpDTO.FirstName,
                LastName = signUpDTO.LastName,
                DateOfBirth = signUpDTO.DateOfBirth,
                Email = signUpDTO.Email,
                PhoneNumber = signUpDTO.PhoneNumber,
                Role = "user"
            };
            _user.PasswordHash = HashPassword(signUpDTO.PasswordHash);
            var _transation = await _repository.BeginTransactionAsync();
            try
            {
                await _repository.AddUserAsync(_user);
                await _transation.CommitAsync();
            }
            catch (UserSaveFailedException)
            {
                await _transation.RollbackAsync();
                throw new UserCreateFailedException();
            }
            await _kafkaProducerService.SendMessageAsync("user-created", _user.Id.ToString());
        }
        public async Task CreateAdminAsync(SignUpDTO signUpDTO)
        {
            if (signUpDTO.DateOfBirth > DateTime.Now)
            {
                throw new FutureDateOfBirthException();
            }
            if (signUpDTO.DateOfBirth > DateTime.Now.AddYears(-18))
            {
                throw new UnderageRegistrationException();
            }
            var user_by_phone = await GetByPhoneAsync(signUpDTO.PhoneNumber);
            if (user_by_phone != null)
            {
                throw new UserPhoneAlreadyExist();
            }
            var user_by_email = await GetByEmailAsync(signUpDTO.Email);
            if (user_by_email != null)
            {
                throw new UserEmailAlreadyExist();
            }
            var _user = new User
            {
                FirstName = signUpDTO.FirstName,
                LastName = signUpDTO.LastName,
                DateOfBirth = signUpDTO.DateOfBirth,
                Email = signUpDTO.Email,
                PhoneNumber = signUpDTO.PhoneNumber,
                Role = "admin"
            };
            _user.PasswordHash = HashPassword(signUpDTO.PasswordHash);
            var _transation = await _repository.BeginTransactionAsync();
            try
            {
                await _repository.AddUserAsync(_user);
                await _transation.CommitAsync();
            }
            catch (UserSaveFailedException)
            {
                await _transation.RollbackAsync();
                throw new UserCreateFailedException();
            }
            await _kafkaProducerService.SendMessageAsync("user-created", _user.Id.ToString());
        }
        public async Task<object> AuthorizeUserAsync(SignInDTO signInDTO)
        {
            var _user = await GetByPhoneAsync(signInDTO.PhoneNumber);
            if (_user == null)
            {
                throw new UserNotFoundException();
            }
            var isCorrectedPassword = CheckPasswordAsync(_user, signInDTO.PasswordHash);
            if (isCorrectedPassword == false)
            {
                throw new InvalidPasswordException();
            }
            var rt = _tokenService.GenerateRefreshToken(_user, signInDTO.DeviceID);
            var response = new
            {
                access_token = _tokenService.GenerateAccessToken(_user),
                refresh_token = rt.Token
            };
            await _tokenService.SaveRefreshTokenAsync(rt);
            return response;
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
