using Identity.DTOs;
using Identity.Services;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;
        public LoginController(IUserService userService, ITokenService tokenService)
        {
            _userService = userService;
            _tokenService = tokenService;
        }
        [HttpPost("SignIn")]
        public async Task<IActionResult> SignIn([FromBody] SignInDTO signInDTO)
        {
            var _user = await _userService.GetByPhoneAsync(signInDTO.PhoneNumber);
            if (_user == null)
            {
                return NotFound("Пользователь не найден!");
            }
            var isCorrectedPassword = _userService.CheckPasswordAsync(_user, signInDTO.PasswordHash);
            if (isCorrectedPassword == false)
            {
                return Unauthorized("Был введён неверный пароль!");
            }
            var rt = _tokenService.GenerateRefreshToken(_user, signInDTO.DeviceID);
            var response = new 
            {
                access_token = _tokenService.GenerateAccessToken(_user),
                refresh_token = rt.Token
            };
            await _tokenService.SaveRefreshTokenAsync(rt);
            return Ok(response);
        }
    }
}
