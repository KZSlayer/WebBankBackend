using Payments.DTOs.KafkaDTOs;
using Payments.Messaging;
using Payments.Models;
using Payments.Repositories;
using Payments.Services.Exceptions;
using System.Security.Claims;

namespace Payments.Services.BaseServices
{
    public class PaymentTransactionService : IPaymentTransactionService
    {
        private readonly IPaymentTransactionRepository _repository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IKafkaProducerService _producerService;
        public PaymentTransactionService(IPaymentTransactionRepository repository, IKafkaProducerService producerService, IHttpContextAccessor httpContextAccessor)
        {
            _repository = repository;
            _producerService = producerService;
            _httpContextAccessor = httpContextAccessor;
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
            try
            {
                var current_pt = await _repository.GetPaymentTransactionById(transactionID);
                if (current_pt == null)
                {
                    throw new PaymentTransactionNotFoundException();
                }
                current_pt.Status = status;
                await _repository.UpdatePaymentTransactionAsync(current_pt);
            }
            catch (PaymentTransactionNotFoundException)
            {
                throw;
            }
            catch (PaymentTransactionUpdateException)
            {
                throw;
            }
        }

        public async Task<List<PaymentTransaction>> GetAllPaymentTransactionsAsync()
        {
            return await _repository.SelectAllPaymentTransactionsAsync();
        }

        public async Task<List<PaymentTransaction>> GetAllUserPaymentTransactionsAsync()
        {
            try
            {
                var userId = _httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrWhiteSpace(userId))
                {
                    throw new UserNotAuthenticatedException();
                }
                return await _repository.SelectAllUserPaymentTransactionsAsync(int.Parse(userId));
            }
            catch (UserNotAuthenticatedException)
            {
                throw;
            }
        }
    }
}
