using Identity.DTOs;
using Identity.Models;
using Identity.Repositories;
using Identity.Security;
using Identity.Services.Exceptions;
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
                    new Claim(ClaimTypes.Role, user.Role)
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
                    new Claim(ClaimTypes.Role, user.Role)
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
        public async Task<object> UpdateAccessTokenAsync(RefreshTokenDTO refreshTokenDTO)
        {
            if (string.IsNullOrEmpty(refreshTokenDTO.RefreshToken) || string.IsNullOrEmpty(refreshTokenDTO.DeviceID))
            {
                throw new ArgumentException();
            }
            var userID = await ValidateRefreshTokenAsync(refreshTokenDTO.RefreshToken, refreshTokenDTO.DeviceID);
            if (userID == null)
            {
                throw new InvalidRefreshTokenException();
            }
            var user = new User { Id = userID.Value };
            var newRefreshToken = GenerateRefreshToken(user, refreshTokenDTO.DeviceID);
            await InvalidationTokenAsync(user.Id, refreshTokenDTO.DeviceID);
            await SaveRefreshTokenAsync(newRefreshToken);
            var tokens = new
            {
                access_token = GenerateAccessToken(user),
                refresh_token = newRefreshToken.Token,
            };
            return tokens;
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
