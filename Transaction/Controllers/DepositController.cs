using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Transaction.DTOs;
using Transaction.Services;

namespace Transaction.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DepositController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly ITransactionsService _transactionsService;
        public DepositController(IAccountService accountService, ITransactionsService transactionsService)
        {
            _accountService = accountService;
            _transactionsService = transactionsService;
        }
        [HttpPost("TopUp")]
        public async Task<IActionResult> TopUp([FromBody] DepositDTO depositDTO)
        {
            try
            {
                await _accountService.FindByIdAsync(depositDTO.UserId);
                await _accountService.IncreaseBalanceAsync(depositDTO.UserId, depositDTO.Amount);
                await _transactionsService.CreateTransactionAsync(null, depositDTO.UserId, depositDTO.Amount, 2);
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
