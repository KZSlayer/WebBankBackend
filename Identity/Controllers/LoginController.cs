using Identity.DTOs;
using Identity.Filters;
using Identity.Services;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [ServiceFilter(typeof(CustomExceptionFilter))]
    public class LoginController : ControllerBase
    {
        private readonly IUserService _userService;
        public LoginController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpPost("SignIn")]
        public async Task<IActionResult> SignIn([FromBody] SignInDTO signInDTO)
        {
            var response = await _userService.AuthorizeUserAsync(signInDTO);
            return Ok(response);
        }
    }
}
