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
    public class AdminController : ControllerBase
    {
        private readonly IUserService _userService;
        public AdminController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("GetAllUsers")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUsers();
            return Ok(users);
        }

        [HttpGet("GetUser")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetUser([FromBody] UserPhoneDTO userPhoneDTO)
        {
            var user = await _userService.GetUserByPhoneAsync(userPhoneDTO.PhoneNumber);
            return Ok(user);
        }
    }
}
