using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Transaction.DTOs;
using Transaction.Filters;
using Transaction.Services;

namespace Transaction.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [ServiceFilter(typeof(CustomExceptionFilter))]
    public class WithdrawController : ControllerBase
    {
        private readonly IAccountTransactionService accountTransactionService;
        public WithdrawController(IAccountTransactionService _accountTransactionService)
        {
            accountTransactionService = _accountTransactionService;
        }
        [HttpPost("CashOut")]
        [Authorize]
        public async Task<IActionResult> CashOut([FromBody] WithdrawDTO withdrawDTO)
        {
            await accountTransactionService.CashOutAccount(withdrawDTO);
            return Ok();
        }
    }
}
