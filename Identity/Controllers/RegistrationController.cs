using Identity.DTOs;
using Identity.Models;
using Identity.Services;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RegistrationController : ControllerBase
    {
        private readonly IUserService _userService;
        public RegistrationController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpPost("SignUp")]
        public async Task<IActionResult> SignUp([FromBody] SignUpDTO user)
        {
            try
            {
                if (user.DateOfBirth > DateTime.Now)
                {
                    return BadRequest("Дата рождения не может быть в будущем.");
                }
                if (user.DateOfBirth > DateTime.Now.AddYears(-18))
                {
                    return BadRequest("Вам должно быть минимум 18 лет для регистрации.");
                }
                var user_by_phone = await _userService.GetByPhoneAsync(user.PhoneNumber);
                var user_by_email = await _userService.GetByEmailAsync(user.Email);
                if (user_by_phone != null)
                {
                    return Conflict("Пользователь с таким номером телефона уже зарегистрирован!");
                }
                if (user_by_email != null)
                {
                    return Conflict("Пользователь с такой почтой уже зарегистрирован!");
                }
                var _user = _userService.ConvertToUser(user);
                _user.PasswordHash = _userService.HashPassword(user.PasswordHash);
                await _userService.CreateUserAsync(_user);
                return Ok("Пользователь успешно зарегистрирован!");
            }
            catch (Exception)
            {
                return StatusCode(500, "Внутренняя ошибка сервера!");
            }
            
        }
    }
}
