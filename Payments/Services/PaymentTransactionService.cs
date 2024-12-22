using Payments.DTOs;
using Payments.Messaging;
using Payments.Models;
using Payments.Repositories;
using Payments.Services.Exceptions;

namespace Payments.Services
{
    public class PaymentTransactionService : IPaymentTransactionService
    {
        private readonly IPaymentTransactionRepository _repository;
        private readonly IKafkaProducerService _producerService;
        public PaymentTransactionService(IPaymentTransactionRepository repository, IKafkaProducerService producerService)
        {
            _repository = repository;
            _producerService = producerService;
        }

        public async Task CreatePaymentTransactionAsync(int userID, int serviceCategoryId, decimal amount)
        {
            var paymentTransaction = new PaymentTransaction
            {
                UserId = userID,
                ServiceCategoryId = serviceCategoryId,
                Amount = amount,
                Currency = "RUB",
                Status = "В обработке",
                Timestamp = DateTime.UtcNow
            };
            var _transaction = await _repository.BeginTransactionAsync();
            try
            {
                await _repository.AddPaymentTransactionAsync(paymentTransaction);
                await _transaction.CommitAsync();
                var kafkaMessage = new ProducerKafkaDTO
                {
                    PaymentTransactionId = paymentTransaction.Id,
                    UserId = paymentTransaction.UserId,
                    Amount = paymentTransaction.Amount,
                };
                await _producerService.SendMessageAsync("payment-transaction-check", kafkaMessage);
            }
            catch (PaymentTransactionAddException)
            {
                await _transaction.RollbackAsync();
                throw;
            }
        }
        public async Task UpdatePaymentTransactionStatusAsync(int transactionID, string status)
        {
            var current_pt = await _repository.GetPaymentTransactionById(transactionID);
            if (current_pt == null)
            {
                throw new PaymentTransactionNotFoundException();
            }
            var transaction = await _repository.BeginTransactionAsync();
            try
            {
                await _repository.EditPaymentTransactionStatusAsync(transactionID, status);
                await transaction.CommitAsync();
            }
            catch (PaymentTransactionUpdateException)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
