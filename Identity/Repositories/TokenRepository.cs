using Identity.Data;
using Identity.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Identity.Repositories
{
    public class TokenRepository : ITokenRepository
    {
        private readonly IdentityDbContext _context;
        private readonly ILogger<TokenRepository> _logger;
        public TokenRepository(IdentityDbContext context, ILogger<TokenRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task AddRefreshTokenAsync(RefreshToken refreshToken)
        {
            try
            {
                await _context.refresh_tokens.AddAsync(refreshToken);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError($"Ошибка при добавлении refresh token в базу данных! \nДетали:\n {ex.Message}");
                throw;
            }
            catch (OperationCanceledException ex)
            {
                _logger.LogError($"Ошибка при добавлении refresh token в базу данных! \nДетали:\n {ex.Message}");
                throw;
            }
        }

        public async Task<RefreshToken?> GetRefreshTokenAsync(string refreshToken, string deviceID)
        {
            return await _context.refresh_tokens
                .FirstOrDefaultAsync(rt => rt.Token == refreshToken
                && rt.DeviceID == deviceID
                && rt.ExpiryDate > DateTime.UtcNow
                && rt.RevokedAt == null);
        }
        public async Task<RefreshToken?> GetRefreshTokenAsync(int userID, string deviceID)
        {
            var token = await _context.refresh_tokens
                .FirstOrDefaultAsync(rt => rt.User.Id == userID
                && rt.DeviceID == deviceID
                && rt.ExpiryDate > DateTime.UtcNow
                && rt.RevokedAt == null);
            return token;
        }
        public async Task<List<RefreshToken>> GetAllUserRefreshTokensAsync(int userId)
        {
            var tokens = await _context.refresh_tokens
                .Where(rt => rt.UserId == userId
                && rt.ExpiryDate > DateTime.UtcNow
                && rt.RevokedAt == null)
                .ToListAsync();
            return tokens;
        }
        public async Task RevokeTokenAsync()
        {
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError($"Ошибка в базе данных при отзыве токена! \nДетали:\n {ex.Message}");
                throw;
            }
            catch (OperationCanceledException ex)
            {
                _logger.LogError($"Ошибка в базе данных при отзыве токена! \nДетали:\n {ex.Message}");
                throw;
            }
        }
        public async Task RevokeAllTokensAsync()
        {
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError($"Ошибка в базе данных при отзыве всех токенов! \nДетали:\n {ex.Message}");
                throw;
            }
            catch (OperationCanceledException ex)
            {
                _logger.LogError($"Ошибка в базе данных при отзыве всех токенов! \nДетали:\n {ex.Message}");
                throw;
            }
        }
        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await _context.Database.BeginTransactionAsync();
        }
    }
}
