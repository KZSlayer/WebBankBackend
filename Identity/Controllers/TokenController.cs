using Identity.DTOs;
using Identity.Models;
using Identity.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TokenController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        public TokenController(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }
        [HttpPost("RefreshTokens")]
        public async Task<IActionResult> GenerateAccessToken([FromBody] RefreshTokenDTO refreshTokenDTO)
        {
            try
            {
                if (string.IsNullOrEmpty(refreshTokenDTO.RefreshToken) || string.IsNullOrEmpty(refreshTokenDTO.DeviceID))
                {
                    return BadRequest("Токены должны быть переданы");
                }
                var userID = await _tokenService.ValidateRefreshTokenAsync(refreshTokenDTO.RefreshToken, refreshTokenDTO.DeviceID);
                if (userID == null)
                {
                    return Unauthorized("Неверный или истёкший токен");
                }
                var user = new User { Id = userID.Value };
                var newRefreshToken = _tokenService.GenerateRefreshToken(user, refreshTokenDTO.DeviceID);
                await _tokenService.InvalidationTokenAsync(user.Id, refreshTokenDTO.DeviceID);
                await _tokenService.SaveRefreshTokenAsync(newRefreshToken);
                var response = new
                {
                    access_token = _tokenService.GenerateAccessToken(user),
                    refresh_token = newRefreshToken.Token,
                };
                return Ok(response);
            }
            catch (Exception)
            {
                return BadRequest("где-то тут ошибка");
            }
            
        }
    }
}
