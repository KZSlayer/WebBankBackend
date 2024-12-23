using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Transaction.DTOs;
using Transaction.Filters;
using Transaction.Services;
using Transaction.Services.BaseServices;

namespace Transaction.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [ServiceFilter(typeof(CustomExceptionFilter))]
    public class UserController : Controller
    {
        private readonly IAccountTransactionService accountTransactionService;
        public UserController(IAccountTransactionService _accountTransactionService)
        {
            accountTransactionService = _accountTransactionService;
        }

        [HttpPost("TopUp")]
        [Authorize(Roles = "user")]
        public async Task<IActionResult> TopUp([FromBody] DepositDTO depositDTO)
        {
            await accountTransactionService.TopUpAccount(depositDTO);
            return Ok();
        }

        [HttpPost("CashOut")]
        [Authorize(Roles = "user")]
        public async Task<IActionResult> CashOut([FromBody] WithdrawDTO withdrawDTO)
        {
            await accountTransactionService.CashOutAccount(withdrawDTO);
            return Ok();
        }

        [HttpPost("TransferByPhone")]
        [Authorize(Roles = "user")]
        public async Task<IActionResult> TransferByPhone([FromBody] TransferByPhoneDTO transferDTO)
        {
            await accountTransactionService.TransferByPhone(transferDTO);
            return Ok();
        }

        [HttpGet("GetMyTransactions")]
        [Authorize(Roles = "user")]
        public async Task<IActionResult> GetUserTransaction()
        {
            var transactions = await accountTransactionService.GetAccountTransaction();
            return Ok(transactions);
        }
    }
}
