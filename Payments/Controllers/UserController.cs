using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Payments.Filters;
using Payments.Services.BaseServices;

namespace Payments.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [ServiceFilter(typeof(CustomExceptionFilter))]
    public class UserController : ControllerBase
    {
        private readonly IPaymentTransactionService _paymentTransactionService;
        public UserController(IPaymentTransactionService paymentTransactionService)
        {
            _paymentTransactionService = paymentTransactionService;
        }
        [HttpGet("GetMyPaymentTransactions")]
        [Authorize(Roles = "user")]
        public async Task<IActionResult> GetAllUserPaymentTransactions()
        {
            var pt = await _paymentTransactionService.GetAllUserPaymentTransactionsAsync();
            return Ok(pt);
        }
    }
}
