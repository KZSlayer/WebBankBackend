using Payments.Models;
using Payments.Repositories;

namespace Payments.Services
{
    public class PaymentTransactionService : IPaymentTransactionService
    {
        private readonly IPaymentTransactionRepository _repository;
        public PaymentTransactionService(IPaymentTransactionRepository repository)
        {
            _repository = repository;
        }

        public async Task CreatePaymentTransactionAsync(int userID, int serviceCategoryId, decimal amount)
        {
            var paymentTransaction = new PaymentTransaction
            {
                UserId = userID,
                ServiceCategoryId = serviceCategoryId,
                Amount = amount,
                Currency = "RUB",
                Status = "Успешно",
                Timestamp = DateTime.UtcNow
            };
            var _transaction = await _repository.BeginTransactionAsync();
            try
            {
                await _repository.AddPaymentTransactionAsync(paymentTransaction);
                await _transaction.CommitAsync();
            }
            catch (Exception)
            {
                await _transaction.RollbackAsync();
                throw new Exception();
            }
        }
    }
}
