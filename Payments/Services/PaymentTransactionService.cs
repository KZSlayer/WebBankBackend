using Payments.DTOs;
using Payments.Messaging;
using Payments.Models;
using Payments.Repositories;

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
                Console.WriteLine($"paymentTransaction.Id: {paymentTransaction.Id}");
                var kafkaMessage = new ProducerKafkaDTO
                {
                    PaymentTransactionId = paymentTransaction.Id,
                    UserId = paymentTransaction.UserId,
                    Amount = paymentTransaction.Amount,
                };
                await _producerService.SendMessageAsync("payment-transaction-check", kafkaMessage);
            }
            catch (Exception)
            {
                await _transaction.RollbackAsync();
                throw new Exception();
            }
        }
        public async Task UpdatePaymentTransactionStatusAsync(int transactionID, string status)
        {
            var transaction = await _repository.BeginTransactionAsync();
            try
            {
                await _repository.EditPaymentTransactionStatusAsync(transactionID, status);
                await transaction.CommitAsync();
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
