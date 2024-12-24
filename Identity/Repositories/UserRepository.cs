using Identity.Data;
using Identity.DTOs;
using Identity.Models;
using Identity.Repositories.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Identity.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IdentityDbContext _context;
        private readonly ILogger<UserRepository> _logger;
        public UserRepository(IdentityDbContext context, ILogger<UserRepository> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task AddUserAsync(User user)
        {
            try
            {
                await _context.users.AddAsync(user);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError($"Ошибка при добавлении пользователя в базу данных! Основная причина: {ex.InnerException?.Message}. Все детали: {ex}");
                throw new UserSaveFailedException("Не удалось сохранить пользователя в базе данных.");
            }
            catch (OperationCanceledException ex)
            {
                _logger.LogError($"Ошибка при добавлении пользователя в базу данных! Основная причина: {ex.InnerException?.Message}. Все детали: {ex}");
                throw;
            }
        }
        public async Task UpdateEmailAsync(User user)
        {
            try
            {
                _context.users.Update(user);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError($"Ошибка при обновлении почты пользователя в базе данных! Основная причина: {ex.InnerException?.Message}. Все детали: {ex}");
                throw;
            }
            catch (OperationCanceledException ex)
            {
                _logger.LogError($"Ошибка при обновлении почты пользователя в базе данных! Основная причина: {ex.InnerException?.Message}. Все детали: {ex}");
                throw;
            }
        }

        public async Task UpdatePhoneAsync(User user)
        {
            try
            {
                _context.users.Update(user);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError($"Ошибка при обновлении номера телефона пользователя в базе данных! Основная причина: {ex.InnerException?.Message}. Все детали: {ex}");
                throw;
            }
            catch (OperationCanceledException ex)
            {
                _logger.LogError($"Ошибка при обновлении номера телефона пользователя в базе данных! Основная причина: {ex.InnerException?.Message}. Все детали: {ex}");
                throw;
            }
        }

        public async Task UpdatePasswordAsync(User user)
        {
            try
            {
                _context.users.Update(user);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError($"Ошибка при обновлении пароля пользователя в базе данных! Основная причина: {ex.InnerException?.Message}. Все детали: {ex}");
                throw;
            }
            catch (OperationCanceledException ex)
            {
                _logger.LogError($"Ошибка при обновлении пароля пользователя в базе данных! Основная причина: {ex.InnerException?.Message}. Все детали: {ex}");
                throw;
            }
        }

        public async Task UpdatePassportDetailsAsync(User user)
        {
            try
            {
                _context.users.Update(user);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError($"Ошибка при обновлении паспортных данных пользователя в базе данных! Основная причина: {ex.InnerException?.Message}. Все детали: {ex}");
                throw;
            }
            catch (OperationCanceledException ex)
            {
                _logger.LogError($"Ошибка при обновлении паспортных данных пользователя в базе данных! Основная причина: {ex.InnerException?.Message}. Все детали: {ex}");
                throw;
            }
        }
        public async Task<List<UserDataDTO>> SelectAllUsersAsync()
        {
            var users = await _context.users
                .Select(u => new UserDataDTO
                {
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    DateOfBirth = u.DateOfBirth,
                    Email = u.Email,
                    PhoneNumber = u.PhoneNumber,
                })
                .ToListAsync();
            return users;
        }
        public async Task<UserDataDTO?> SelectUserByPhoneAsync(string phoneNumber)
        {
            var user = await _context.users
                .Where(u => u.PhoneNumber == phoneNumber)
                .Select(u => new UserDataDTO
                {
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    DateOfBirth = u.DateOfBirth,
                    Email = u.Email,
                    PhoneNumber = u.PhoneNumber,
                })
                .FirstOrDefaultAsync();
            return user;
        }
        public async Task<User?> SelectByIdAsync(int userId)
        {
            return await _context.users.FirstOrDefaultAsync(u => u.Id == userId);
        }
        public async Task<User?> SelectByPhoneAsync(string phoneNumber)
        {
            return await _context.users.FirstOrDefaultAsync(u => u.PhoneNumber == phoneNumber);
        }

        public async Task<User?> SelectByEmailAsync(string email)
        {
            return await _context.users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await _context.Database.BeginTransactionAsync();
        }
    }
}
