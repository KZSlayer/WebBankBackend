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
    public class DepositController : ControllerBase
    {
        private readonly IAccountTransactionService accountTransactionService;
        public DepositController(IAccountTransactionService _accountTransactionService)
        {
            accountTransactionService = _accountTransactionService;
        }
        [HttpPost("TopUp")]
        [Authorize]
        public async Task<IActionResult> TopUp([FromBody] DepositDTO depositDTO)
        {
            await accountTransactionService.TopUpAccount(depositDTO);
            return Ok();
        }
    }
}
