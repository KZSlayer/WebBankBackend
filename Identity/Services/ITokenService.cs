using Identity.Models;

namespace Identity.Services
{
    public interface ITokenService
    {
        string GenerateAccessToken(User user);
        RefreshToken GenerateRefreshToken(User user, string deviceID);
        Task SaveRefreshTokenAsync(RefreshToken refreshToken);
        Task<int?> ValidateRefreshTokenAsync(string refreshToken, string deviceID);
        Task InvalidationTokenAsync(int userID, string deviceID);
        Task InvalidationAllTokensAsync(int userID);
    }
}
