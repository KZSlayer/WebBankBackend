using Microsoft.AspNetCore.Mvc;
using Payments.DTOs;
using Payments.Filters;
using Payments.Services;
using Payments.Services.BaseServices;

namespace Payments.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [ServiceFilter(typeof(CustomExceptionFilter))]
    public class PayPhoneController : ControllerBase
    {
        private readonly IPayPhoneService _payPhoneService;
        public PayPhoneController(IPayPhoneService payPhoneService)
        {
            _payPhoneService = payPhoneService;
        }

        [HttpPost("PayPhone")]
        public async Task<IActionResult> PayPhone([FromBody] PayPhoneDTO payPhoneDTO)
        {
            await _payPhoneService.PayPhoneAsync(payPhoneDTO);
            return Ok();
        }
    }
}
