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
    public class ChangeDataController : ControllerBase
    {
        private readonly IUserService _userService;
        public ChangeDataController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPatch("ChangeEmail")]
        [Authorize(Roles = "user, admin")]
        public async Task<IActionResult> ChangeEmail([FromBody] ChangeEmailDTO changeEmailDTO)
        {
            await _userService.EditEmailAsync(changeEmailDTO);
            return Ok();
        }
        [HttpPatch("ChangePhone")]
        [Authorize(Roles = "user, admin")]
        public async Task<IActionResult> ChangePhone([FromBody] ChangePhoneDTO changePhoneDTO)
        {
            await _userService.EditPhoneAsync(changePhoneDTO);
            return Ok();
        }
        [HttpPatch("ChangePassword")]
        [Authorize(Roles = "user, admin")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDTO changePasswordDTO)
        {
            await _userService.EditPasswordAsync(changePasswordDTO);
            return Ok();
        }
        [HttpPatch("ChangePassportDetails")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> ChangePassportDetails([FromBody] ChangePassportDetailsDTO changePassportDetailsDTO)
        {
            await _userService.EditPassportDetailsAsync(changePassportDetailsDTO);
            return Ok();
        }
    }
}
