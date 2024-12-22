using Identity.DTOs;
using Identity.Filters;
using Identity.Models;
using Identity.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [ServiceFilter(typeof(CustomExceptionFilter))]
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
            var new_tokens = await _tokenService.UpdateAccessTokenAsync(refreshTokenDTO);
            return Ok(new_tokens);
        }
    }
}
