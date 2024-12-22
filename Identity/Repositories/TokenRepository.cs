using Identity.Data;
using Identity.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Identity.Repositories
{
    public class TokenRepository : ITokenRepository
    {
        private readonly IdentityDbContext _context;
        public TokenRepository(IdentityDbContext context)
        {
            _context = context;
        }

        public async Task AddRefreshTokenAsync(RefreshToken refreshToken)
        {
            await _context.refresh_tokens.AddAsync(refreshToken);
            await _context.SaveChangesAsync();
        }

        public async Task<RefreshToken?> GetRefreshTokenAsync(string refreshToken, string deviceID)
        {
            return await _context.refresh_tokens
                .FirstOrDefaultAsync(rt => rt.Token == refreshToken
                && rt.DeviceID == deviceID
                && rt.ExpiryDate > DateTime.UtcNow
                && rt.RevokedAt == null);
        }

        public async Task RevokeTokenAsync(int userID, string deviceID)
        {
            var token = await _context.refresh_tokens
                .FirstOrDefaultAsync(rt => rt.User.Id == userID
                && rt.DeviceID == deviceID
                && rt.ExpiryDate > DateTime.UtcNow
                && rt.RevokedAt == null);
            if (token != null)
            {
                token.RevokedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
        }
        public async Task RevokeAllTokensAsync(int userID)
        {
            var tokens = _context.refresh_tokens
                .Where(rt => rt.UserId == userID
                && rt.ExpiryDate > DateTime.UtcNow
                && rt.RevokedAt == null);
            foreach (var token in tokens)
            {
                token.RevokedAt = DateTime.UtcNow;
            }
            await _context.SaveChangesAsync();
        }
        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await _context.Database.BeginTransactionAsync();
        }
    }
}
