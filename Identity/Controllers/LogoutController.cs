using Identity.DTOs;
using Identity.Filters;
using Identity.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [ServiceFilter(typeof(CustomExceptionFilter))]
    public class LogoutController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        public LogoutController(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }

        [HttpPost("FromDevice")]
        [Authorize(Roles = "user, admin")]
        public async Task<IActionResult> LogoutFromDevice([FromBody] SignOutDTO signOutDTO)
        {
            await _tokenService.InvalidationTokenAsync(signOutDTO.UserID, signOutDTO.DeviceID);
            return Ok();
        }

        [HttpPost("FromAllDevices")]
        [Authorize(Roles = "user, admin")]
        public async Task<IActionResult> LogoutFromAllDevices([FromBody] SignOutDTO signOutDTO)
        {
            await _tokenService.InvalidationAllTokensAsync(signOutDTO.UserID);
            return Ok();
        }
    }
}
