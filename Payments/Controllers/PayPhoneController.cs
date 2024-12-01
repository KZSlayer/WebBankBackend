using Microsoft.AspNetCore.Mvc;
using Payments.Services;

namespace Payments.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PayPhoneController : ControllerBase
    {
        private readonly IPhoneNumberRangesService _phoneNumberRangesService;
        public PayPhoneController(IPhoneNumberRangesService phoneNumberRangesService)
        {
            _phoneNumberRangesService = phoneNumberRangesService;
        }

        [HttpPost("PayPhone")]
        public async Task<IActionResult> PayPhone(string phoneNumber)
        {
            try
            {
                var providerID = await _phoneNumberRangesService.FindPaymentProviderIdAsync(phoneNumber);
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest("Ошибка");
            }
        }
    }
}
