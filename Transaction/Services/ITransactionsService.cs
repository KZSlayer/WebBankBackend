using Transaction.Models;

namespace Transaction.Services
{
    public interface ITransactionsService
    {
        Task CreateTransactionAsync(int? fromAccountID, int? toAccountID, decimal amount, int transactionTypeID);
    }
}
