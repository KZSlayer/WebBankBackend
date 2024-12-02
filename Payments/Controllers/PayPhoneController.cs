using Microsoft.AspNetCore.Mvc;
using Payments.Services;

namespace Payments.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PayPhoneController : ControllerBase
    {
        private readonly IPhoneNumberRangesService _phoneNumberRangesService;
        private readonly IPaymentProviderService _paymentProviderService;
        private readonly IPaymentTransactionService _paymentTransactionService;
        public PayPhoneController(IPhoneNumberRangesService phoneNumberRangesService, IPaymentTransactionService paymentTransactionService, IPaymentProviderService paymentProviderService)
        {
            _phoneNumberRangesService = phoneNumberRangesService;
            _paymentTransactionService = paymentTransactionService;
            _paymentProviderService = paymentProviderService;
        }

        [HttpPost("PayPhone")]
        public async Task<IActionResult> PayPhone(int userID, string phoneNumber, decimal amount)
        {
            try
            {
                var providerID = await _phoneNumberRangesService.FindPaymentProviderIdAsync(phoneNumber);
                var categoryID = await _paymentProviderService.FindServiceCategoryIdAsync(providerID.Value);
                await _paymentTransactionService.CreatePaymentTransactionAsync(userID, categoryID.Value, amount);
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest("Ошибка");
            }
        }
    }
}
