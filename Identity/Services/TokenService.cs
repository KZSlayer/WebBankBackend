using Identity.Models;
using Identity.Repositories;
using Identity.Security;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Identity.Services
{
    public class TokenService : ITokenService
    {
        private readonly ITokenRepository _repository;
        public TokenService(ITokenRepository repository)
        {
            _repository = repository;
        }

        public string GenerateAccessToken(User user)
        {
            try
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                };

                var jwt = new JwtSecurityToken(
                        issuer: JwtAuthOptions.ISSUER,
                        audience: JwtAuthOptions.AUDIENCE,
                        claims: claims,
                expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(5)),
                        signingCredentials: new SigningCredentials(JwtAuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
                return new JwtSecurityTokenHandler().WriteToken(jwt);
            }
            catch (Exception)
            {
                Console.WriteLine("Ошибка GenerateAccessToken");
                throw;
            }
            
        }

        public RefreshToken GenerateRefreshToken(User user, string deviceID)
        {
            try
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                };

                var jwt = new JwtSecurityToken(
                        issuer: JwtAuthOptions.ISSUER,
                        audience: JwtAuthOptions.AUDIENCE,
                        claims: claims,
                expires: DateTime.UtcNow.Add(TimeSpan.FromDays(30)),
                        signingCredentials: new SigningCredentials(JwtAuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
                return new RefreshToken
                {
                    UserId = user.Id,
                    DeviceID = deviceID,
                    Token = new JwtSecurityTokenHandler().WriteToken(jwt),
                    ExpiryDate = DateTime.UtcNow.AddDays(30),
                    CreatedAt = DateTime.UtcNow
                };
            }
            catch (Exception)
            {
                Console.WriteLine("Ошибка GenerateRefreshToken");
                throw;
            }
            
        }

        public async Task<int?> ValidateRefreshTokenAsync(string refreshToken, string deviceID)
        {
            try
            {
                var token = await _repository.GetRefreshTokenAsync(refreshToken, deviceID);
                if (token == null)
                {
                    return null;
                }
                return token.UserId;
            }
            catch (Exception)
            {
                Console.WriteLine("Ошибка ValidateRefreshTokenAsync");
                throw;
            }
        }
        public async Task InvalidationAllTokensAsync(int userID)
        {
            var _transaction = await _repository.BeginTransactionAsync();
            try
            {
                await _repository.RevokeAllTokensAsync(userID);
                await _transaction.CommitAsync();
            }
            catch (Exception)
            {
                await _transaction.RollbackAsync();
            }
        }

        public async Task InvalidationTokenAsync(int userID, string deviceID)
        {
            var _transaction = await _repository.BeginTransactionAsync();
            try
            {
                await _repository.RevokeTokenAsync(userID, deviceID);
                await _transaction.CommitAsync();
            }
            catch (Exception)
            {
                await _transaction.RollbackAsync();
            }
        }

        public async Task SaveRefreshTokenAsync(RefreshToken refreshToken)
        {
            var _transaction = await _repository.BeginTransactionAsync();
            try
            {
                await _repository.AddRefreshTokenAsync(refreshToken);
                await _transaction.CommitAsync();
            }
            catch (Exception)
            {
                Console.WriteLine("Ошибка SaveRefreshTokenAsync");
                await _transaction.RollbackAsync();
            }
            
        }
    }
}
