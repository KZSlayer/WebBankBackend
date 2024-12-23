using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Transaction.DTOs;
using Transaction.Messaging;
using Transaction.Services.BaseServices;
using Transaction.Services.Exceptions;

namespace Transaction.Services
{
    public class AccountTransactionService : IAccountTransactionService
    {
        private readonly IAccountService _accountService;
        private readonly ITransactionsService _transactionsService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IKafkaProducerService _producer;
        private readonly PendingRequestsStore _pendingRequestsStore;
        public AccountTransactionService(IAccountService accountService, ITransactionsService transactionsService, IHttpContextAccessor httpContextAccessor, IKafkaProducerService producer, PendingRequestsStore pendingRequestsStore)
        {
            _accountService = accountService;
            _transactionsService = transactionsService;
            _httpContextAccessor = httpContextAccessor;
            _producer = producer;
            _pendingRequestsStore = pendingRequestsStore;
        }

        public async Task TopUpAccount(DepositDTO depositDTO)
        {
            try
            {
                var userId = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
                if (userId == null)
                {
                    throw new UserNotAuthenticatedException();
                }
                if (depositDTO.Amount <= 0)
                {
                    throw new InvalidAmountException();
                }
                var account = await _accountService.FindByUserIdAsync(int.Parse(userId));
                await _accountService.IncreaseBalanceAsync(account, depositDTO.Amount);
                await _transactionsService.CreateTransactionAsync(null, account.AccountNumber, depositDTO.Amount, 2);
            }
            catch (InvalidDataException)
            {
                throw;
            }
            catch (DbUpdateException)
            {
                throw;
            }
            catch (InvalidAmountException)
            {
                throw;
            }
            catch (UserNotAuthenticatedException)
            {
                throw;
            }
            catch (AccountNotFoundException)
            {
                throw;
            }
        }
        public async Task CashOutAccount(WithdrawDTO withdrawDTO)
        {
            try
            {
                var userId = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
                if (userId == null)
                {
                    throw new UserNotAuthenticatedException();
                }
                if (withdrawDTO.Amount <= 0)
                {
                    throw new InvalidAmountException();
                }
                var account = await _accountService.FindByUserIdAsync(int.Parse(userId));
                await _accountService.IncreaseBalanceAsync(account, withdrawDTO.Amount);
                await _transactionsService.CreateTransactionAsync(account.AccountNumber, null, withdrawDTO.Amount, 3);
            }
            catch (InvalidDataException)
            {
                throw;
            }
            catch (DbUpdateException)
            {
                throw;
            }
            catch (InvalidAmountException)
            {
                throw;
            }
            catch (UserNotAuthenticatedException)
            {
                throw;
            }
            catch (AccountNotFoundException)
            {
                throw;
            }
        }

        public async Task TransferByPhone(TransferByPhoneDTO transferDTO)
        {
            var correlationId = Guid.NewGuid().ToString();
            var tcs = new TaskCompletionSource<int?>();
            try
            {
                var userId = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
                if (userId == null)
                {
                    throw new UserNotAuthenticatedException();
                }
                if (transferDTO.Amount <= 0)
                {
                    throw new InvalidAmountException();
                }
                _pendingRequestsStore.AddRequest(correlationId, tcs);
                var query = new CheckPhoneDTO
                {
                    SenderId = int.Parse(userId),
                    PhoneNumber = transferDTO.PhoneNumber,
                    CorrelationId = correlationId,
                };
                await _producer.SendCheckPhoneAsync("phone-query", query);
                var recipientId = await Task.WhenAny(tcs.Task, Task.Delay(TimeSpan.FromSeconds(15))) == tcs.Task
                    ? await tcs.Task
                    : throw new TimeoutException("Не удалось получить ответ от микросервиса Identity.");
                if (recipientId == null)
                {
                    throw new RecipientNotFoundException();
                }
                var sender = await _accountService.FindByUserIdAsync(int.Parse(userId));
                var recipient = await _accountService.FindByUserIdAsync(recipientId.Value);
                await _accountService.DecreaseBalanceAsync(sender, transferDTO.Amount);
                await _accountService.IncreaseBalanceAsync(recipient, transferDTO.Amount);
                await _transactionsService.CreateTransactionAsync(sender.AccountNumber, recipient.AccountNumber, transferDTO.Amount, 1);
            }
            catch (RecipientNotFoundException)
            {
                throw;
            }
            catch (InvalidAmountException)
            {
                throw;
            }
            catch (UserNotAuthenticatedException)
            {
                throw;
            }
            catch (AccountNotFoundException)
            {
                throw;
            }
            finally
            {
                _pendingRequestsStore.TryRemove(correlationId, out _);
            }
        }
        public async Task<List<TransactionDTO>> GetAccountTransaction()
        {
            try
            {
                var userId = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
                if (userId == null)
                {
                    throw new UserNotAuthenticatedException();
                }
                var user = await _accountService.FindByUserIdAsync(int.Parse(userId)); // Проверка на Parse
                var transactions = await _transactionsService.GetAccountTransactionsAsync(user.AccountNumber);
                return transactions;
            }
            catch (UserNotAuthenticatedException)
            {
                throw;
            }
            catch (AccountNotFoundException)
            {
                throw;
            }
        }
    }
}
