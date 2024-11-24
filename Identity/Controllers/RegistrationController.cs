using Identity.DTOs;
using Identity.Filters;
using Identity.Messaging;
using Identity.Models;
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
        private readonly IKafkaProducerService _kafkaProducerService;
        public RegistrationController(IUserService userService, IKafkaProducerService kafkaProducerService)
        {
            _userService = userService;
            _kafkaProducerService = kafkaProducerService;
        }
        [HttpPost("SignUp")]
        public async Task<IActionResult> SignUp([FromBody] SignUpDTO user)
        {
            if (user.DateOfBirth > DateTime.Now)
            {
                //return BadRequest("Дата рождения не может быть в будущем.");
                throw new ArgumentException("Дата рождения не может быть в будущем.");
            }
            if (user.DateOfBirth > DateTime.Now.AddYears(-18))
            {
                //return BadRequest("Вам должно быть минимум 18 лет для регистрации.");
                throw new InvalidOperationException("Вам должно быть минимум 18 лет для регистрации.");
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
            await _kafkaProducerService.SendMessageAsync("user-created", _user.Id);
            return Ok("Пользователь успешно зарегистрирован!");
        }
    }
}
