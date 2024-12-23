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
    public class TransferController : ControllerBase
    {
        private readonly IAccountTransactionService accountTransactionService;
        public TransferController(IAccountTransactionService _accountTransactionService)
        {
            accountTransactionService = _accountTransactionService;
        }
        [HttpPost("TransferByPhone")]
        [Authorize]
        public async Task<IActionResult> TransferByPhone([FromBody] TransferByPhoneDTO transferDTO)
        {
            await accountTransactionService.TransferByPhone(transferDTO);
            return Ok();
        }
    }
}
