using Identity.Models;
using Microsoft.EntityFrameworkCore.Storage;

namespace Identity.Repositories
{
    public interface ITokenRepository
    {
        Task AddRefreshTokenAsync(RefreshToken refreshToken);
        Task<RefreshToken?> GetRefreshTokenAsync(string refreshToken, string deviceID);
        Task RevokeTokenAsync(int userID, string deviceID);
        Task RevokeAllTokensAsync(int userID);
        Task<IDbContextTransaction> BeginTransactionAsync();
    }
}
