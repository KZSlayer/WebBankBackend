using Identity.DTOs;
using Identity.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LogoutController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        public LogoutController(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }

        [HttpPost("FromDevice")]
        [Authorize]
        public async Task<IActionResult> LogoutFromDevice([FromBody] SignOutDTO signOutDTO)
        {
            await _tokenService.InvalidationTokenAsync(signOutDTO.UserID, signOutDTO.DeviceID);
            return Ok();
        }

        [HttpPost("FromAllDevices")]
        [Authorize]
        public async Task<IActionResult> LogoutFromAllDevices([FromBody] SignOutDTO signOutDTO)
        {
            await _tokenService.InvalidationAllTokensAsync(signOutDTO.UserID);
            return Ok();
        }
    }
}
