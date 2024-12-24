using Identity.DTOs;
using Identity.Models;
using Identity.Repositories;
using Identity.Repositories.Exceptions;
using Identity.Security;
using Identity.Services.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Identity.Services
{
    public class TokenService : ITokenService
    {
        private readonly ITokenRepository _repository;
        private readonly ILogger<TokenService> _logger;
        public TokenService(ITokenRepository repository, ILogger<TokenService> logger)
        {
            _repository = repository;
            _logger = logger;
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
            try
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

                var oldRefreshToken = await _repository.GetRefreshTokenAsync(refreshTokenDTO.RefreshToken, refreshTokenDTO.DeviceID);
                oldRefreshToken.RevokedAt = DateTime.UtcNow;
                await _repository.RevokeTokenAsync();

                await _repository.AddRefreshTokenAsync(newRefreshToken);

                var tokens = new
                {
                    access_token = GenerateAccessToken(user),
                    refresh_token = newRefreshToken.Token,
                };
                return tokens;
            }
            catch (ArgumentException)
            {
                _logger.LogError($"Ошибка в UpdateAccessTokenAsync! Переданы пустые аргументы!");
                throw;
            }
            catch (InvalidRefreshTokenException)
            {
                _logger.LogError($"Ошибка в UpdateAccessTokenAsync! userID is null!");
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
        public async Task<int?> ValidateRefreshTokenAsync(string refreshToken, string deviceID)
        {
            var token = await _repository.GetRefreshTokenAsync(refreshToken, deviceID);
            if (token == null)
            {
                return null;
            }
            return token.UserId;
        }
        public async Task InvalidationAllTokensAsync(int userID)
        {
            var _transaction = await _repository.BeginTransactionAsync();
            try
            {
                var tokens = await _repository.GetAllUserRefreshTokensAsync(userID);
                foreach (var token in tokens)
                {
                    token.RevokedAt = DateTime.UtcNow;
                }
                await _repository.RevokeAllTokensAsync();
                await _transaction.CommitAsync();
            }
            catch (DbUpdateException)
            {
                await _transaction.RollbackAsync();
                throw;
            }
            catch (OperationCanceledException)
            {
                await _transaction.RollbackAsync();
                throw;
            }
        }

        public async Task InvalidationTokenAsync(int userID, string deviceID)
        {
            try
            {
                var token = await _repository.GetRefreshTokenAsync(userID, deviceID);
                if (token == null)
                {
                    throw new TokenNotFoundException();
                }
                token.RevokedAt = DateTime.UtcNow;
                await _repository.RevokeTokenAsync();
            }
            catch (TokenNotFoundException)
            {
                _logger.LogError($"Ошибка в InvalidationTokenAsync! token is null!");
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

        public async Task SaveRefreshTokenAsync(RefreshToken refreshToken)
        {
            var _transaction = await _repository.BeginTransactionAsync();
            try
            {
                await _repository.AddRefreshTokenAsync(refreshToken);
                await _transaction.CommitAsync();
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
    }
}
