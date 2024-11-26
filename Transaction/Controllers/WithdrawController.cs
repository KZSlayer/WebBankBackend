using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Transaction.DTOs;
using Transaction.Services;

namespace Transaction.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WithdrawController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly ITransactionsService _transactionsService;
        public WithdrawController(IAccountService accountService, ITransactionsService transactionsService)
        {
            _accountService = accountService;
            _transactionsService = transactionsService;
        }
        [HttpPost("CashOut")]
        public async Task<IActionResult> CashOut([FromBody] WithdrawDTO withdrawDTO)
        {
            try
            {
                await _accountService.FindByIdAsync(withdrawDTO.UserId);
                await _accountService.DecreaseBalanceAsync(withdrawDTO.UserId, withdrawDTO.Amount);
                await _transactionsService.CreateTransactionAsync(withdrawDTO.UserId, null, withdrawDTO.Amount, 3);
                return Ok();
            }
            catch (InvalidDataException)
            {
                return NotFound("Аккаунт не найден");
            }
            catch (DbUpdateException)
            {
                return BadRequest("Данные не сохранились");
            }
        }
    }
}
