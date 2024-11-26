using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;
using Transaction.DTOs;
using Transaction.Services;

namespace Transaction.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TransferController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly ITransactionsService _transactionsService;
        public TransferController(IAccountService accountService, ITransactionsService transactionsService)
        {
            _accountService = accountService;
            _transactionsService = transactionsService;
        }
        [HttpPost("TransferById")]
        public async Task<IActionResult> TransferById([FromBody] TransferDTO transferDTO)
        {
            try
            {
                await _accountService.FindByIdAsync(transferDTO.FromAccountUserId);
                await _accountService.FindByIdAsync(transferDTO.ToAccountUserId);
                await _accountService.DecreaseBalanceAsync(transferDTO.FromAccountUserId, transferDTO.Amount);
                await _accountService.IncreaseBalanceAsync(transferDTO.ToAccountUserId, transferDTO.Amount);
                await _transactionsService.CreateTransactionAsync(transferDTO.FromAccountUserId, transferDTO.ToAccountUserId, transferDTO.Amount, 1);
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
