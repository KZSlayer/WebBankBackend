using Identity.DTOs;
using Identity.Messaging;
using Identity.Models;
using Identity.Repositories;
using Identity.Repositories.Exceptions;
using Identity.Services.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Identity.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;
        private readonly ITokenService _tokenService;
        private readonly IKafkaProducerService _kafkaProducerService;
        private IHttpContextAccessor _httpContextAccessor;
        private readonly PasswordHasher<User> _passwordHasher;
        public UserService(IUserRepository repository, ITokenService tokenService, IKafkaProducerService kafkaProducerService, IHttpContextAccessor contextAccessor)
        {
            _repository = repository;
            _passwordHasher = new PasswordHasher<User>();
            _tokenService = tokenService;
            _kafkaProducerService = kafkaProducerService;
            _httpContextAccessor = contextAccessor;
        }

        public async Task CreateUserAsync(SignUpDTO signUpDTO)
        {
            try
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
                await _repository.AddUserAsync(_user);
                await _kafkaProducerService.SendMessageAsync("user-created", _user.Id.ToString());
            }
            catch (FutureDateOfBirthException)
            {
                throw;
            }
            catch (UnderageRegistrationException)
            {
                throw;
            }
            catch (UserPhoneAlreadyExist)
            {
                throw;
            }
            catch (UserEmailAlreadyExist)
            {
                throw;
            }
            catch (UserSaveFailedException)
            {
                throw;
            }
        }
        public async Task CreateAdminAsync(SignUpDTO signUpDTO)
        {
            try
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
                await _repository.AddUserAsync(_user);
            }
            catch (FutureDateOfBirthException)
            {
                throw;
            }
            catch (UnderageRegistrationException)
            {
                throw;
            }
            catch (UserPhoneAlreadyExist)
            {
                throw;
            }
            catch (UserEmailAlreadyExist)
            {
                throw;
            }
            catch (UserSaveFailedException)
            {
                throw;
            }
        }
        public async Task EditEmailAsync(ChangeEmailDTO changeEmailDTO)
        {
            try
            {
                var userId = _httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
                if (userId == null)
                {
                    throw new UserNotAuthenticatedException();
                }
                var _user = await _repository.SelectByIdAsync(int.Parse(userId));
                if (_user == null)
                {
                    throw new UserNotFoundException();
                }
                var new_email = await _repository.SelectByEmailAsync(changeEmailDTO.Email);
                if (new_email != null)
                {
                    throw new UserEmailAlreadyExist();
                }
                _user.Email = changeEmailDTO.Email;
                await _repository.UpdateEmailAsync(_user);
            }
            catch (UserNotAuthenticatedException)
            {
                throw;
            }
            catch (UserNotFoundException)
            {
                throw;
            }
            catch (UserEmailAlreadyExist)
            {
                throw;
            }
            catch (DbUpdateException)
            {
                throw;
            }
            catch (OperationCanceledException)
            {
                throw;
            }
        }

        public async Task EditPhoneAsync(ChangePhoneDTO changePhoneDTO)
        {
            try
            {
                var userId = _httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
                if (userId == null)
                {
                    throw new UserNotAuthenticatedException();
                }
                var _user = await _repository.SelectByIdAsync(int.Parse(userId));
                if (_user == null)
                {
                    throw new UserNotFoundException();
                }
                var new_phone = await _repository.SelectByPhoneAsync(changePhoneDTO.PhoneNumber);
                if (new_phone != null)
                {
                    throw new UserPhoneAlreadyExist();
                }
                _user.PhoneNumber = changePhoneDTO.PhoneNumber;
                await _repository.UpdatePhoneAsync(_user);
            }
            catch (UserNotAuthenticatedException)
            {
                throw;
            }
            catch (UserNotFoundException)
            {
                throw;
            }
            catch (UserPhoneAlreadyExist)
            {
                throw;
            }
            catch (DbUpdateException)
            {
                throw;
            }
            catch (OperationCanceledException)
            {
                throw;
            }
        }

        public async Task EditPasswordAsync(ChangePasswordDTO changePasswordDTO)
        {
            try
            {
                var userId = _httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
                if (userId == null)
                {
                    throw new UserNotAuthenticatedException();
                }
                var _user = await _repository.SelectByIdAsync(int.Parse(userId));
                if (_user == null)
                {
                    throw new UserNotFoundException();
                }
                var isCorrectedPassword = CheckPasswordAsync(_user, changePasswordDTO.CurrentPassword);
                if (isCorrectedPassword == false)
                {
                    throw new InvalidPasswordException();
                }
                _user.PasswordHash = HashPassword(changePasswordDTO.NewPassword);
                await _repository.UpdatePhoneAsync(_user);
            }
            catch (UserNotAuthenticatedException)
            {
                throw;
            }
            catch (UserNotFoundException)
            {
                throw;
            }
            catch (InvalidPasswordException)
            {
                throw;
            }
            catch (DbUpdateException)
            {
                throw;
            }
            catch (OperationCanceledException)
            {
                throw;
            }
        }

        public async Task EditPassportDetailsAsync(ChangePassportDetailsDTO changePassportDetailsDTO)
        {
            try
            {
                var _user = await _repository.SelectByPhoneAsync(changePassportDetailsDTO.PhoneNumber);
                if (_user == null)
                {
                    throw new UserNotFoundException();
                }
                _user.FirstName = changePassportDetailsDTO.FirstName;
                _user.LastName = changePassportDetailsDTO.LastName;
                _user.DateOfBirth = changePassportDetailsDTO.DateOfBirth;
                await _repository.UpdatePassportDetailsAsync(_user);
            }
            catch (UserNotFoundException)
            {
                throw;
            }
            catch (DbUpdateException)
            {
                throw;
            }
            catch (OperationCanceledException)
            {
                throw;
            }
        }
        public async Task<object> AuthorizeUserAsync(SignInDTO signInDTO)
        {
            try
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
            catch (UserNotFoundException)
            {
                throw;
            }
            catch (InvalidPasswordException)
            {
                throw;
            }
        }

        public async Task<List<UserDataDTO>> GetAllUsers()
        {
            return await _repository.SelectAllUsersAsync();
        }
        public async Task<UserDataDTO> GetUserByPhoneAsync(string phoneNumber)
        {
            try
            {
                var user = await GetUserByPhoneAsync(phoneNumber);
                if (user == null)
                {
                    throw new UserNotFoundException();
                }
                return user;
            }
            catch (UserNotFoundException)
            {
                throw;
            }
        }
        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _repository.SelectByEmailAsync(email);
        }

        public async Task<User?> GetByPhoneAsync(string phoneNumber)
        {
            return await _repository.SelectByPhoneAsync(phoneNumber);
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
