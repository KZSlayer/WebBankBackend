using Identity.DTOs;
using Identity.Filters;
using Identity.Services;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [ServiceFilter(typeof(CustomExceptionFilter))]
    public class RegistrationController : ControllerBase
    {
        private readonly IUserService _userService;
        
        public RegistrationController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpPost("user")]
        public async Task<IActionResult> SignUpUser([FromBody] SignUpDTO user)
        {
            await _userService.CreateUserAsync(user);
            return Ok("Пользователь успешно зарегистрирован!");
        }
        [HttpPost("admin")]
        public async Task<IActionResult> SignUpAdmin([FromBody] SignUpDTO admin)
        {
            await _userService.CreateAdminAsync(admin);
            return Ok("Пользователь успешно зарегистрирован!");
        }
    }
}
