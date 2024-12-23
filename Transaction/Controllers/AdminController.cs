using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Transaction.DTOs;
using Transaction.Filters;
using Transaction.Services.BaseServices;

namespace Transaction.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [ServiceFilter(typeof(CustomExceptionFilter))]
    public class AdminController : ControllerBase
    {
        private readonly ITransactionTypeService transactionTypeService;
        private readonly ITransactionsService transactionsService;
        public AdminController(ITransactionTypeService _transactionTypeService, ITransactionsService _transactionsService)
        {
            transactionTypeService = _transactionTypeService;
            transactionsService = _transactionsService;
        }

        [HttpPost("CreateTransactionType")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> CreateNewTransactionType([FromBody] CreateTransactionTypeDTO createTransactionTypeDTO)
        {
            await transactionTypeService.CreateTransactionTypeAsync(createTransactionTypeDTO);
            return Ok();
        }

        [HttpPost("EditTransactionType")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateExistTransactionType([FromBody] ChangeTransactionTypeDTO changeTransactionTypeDTO)
        {
            await transactionTypeService.UpdateTransactionTypeAsync(changeTransactionTypeDTO);
            return Ok();
        }

        [HttpGet("GetAllTransactions")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetAllTransactions()
        {
            var transactions = await transactionsService.GetTransactionsAsync();
            return Ok(transactions);
        }
    }
}
