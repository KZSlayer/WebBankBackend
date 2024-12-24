using Identity.Models;
using Microsoft.EntityFrameworkCore.Storage;

namespace Identity.Repositories
{
    public interface ITokenRepository
    {
        Task AddRefreshTokenAsync(RefreshToken refreshToken);
        Task<RefreshToken?> GetRefreshTokenAsync(string refreshToken, string deviceID);
        Task<RefreshToken?> GetRefreshTokenAsync(int userID, string deviceID);
        Task<List<RefreshToken>> GetAllUserRefreshTokensAsync(int userId);
        Task RevokeTokenAsync();
        Task RevokeAllTokensAsync();
        Task<IDbContextTransaction> BeginTransactionAsync();
    }
}
