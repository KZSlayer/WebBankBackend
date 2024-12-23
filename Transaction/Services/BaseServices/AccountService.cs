using Microsoft.EntityFrameworkCore;
using Transaction.Models;
using Transaction.Repositories;
using Transaction.Services.Exceptions;

namespace Transaction.Services.BaseServices
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _repository;
        public AccountService(IAccountRepository repository)
        {
            _repository = repository;
        }

        public async Task CreateAccountAsync(Account account)
        {
            var transaction = await _repository.BeginTransactionAsync();
            try
            {
                await _repository.AddAccountAsync(account);
                await transaction.CommitAsync();
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
            }

        }
        public async Task<Account> FindByUserIdAsync(int userID)
        {
            try
            {
                var _account = await _repository.FindByUserIdAsync(userID);
                if (_account == null)
                {
                    throw new AccountNotFoundException();
                }
                return _account;
            }
            catch (AccountNotFoundException)
            {
                throw;
            }

        }
        public async Task IncreaseBalanceAsync(Account account, decimal amount)
        {
            var transaction = await _repository.BeginTransactionAsync();
            try
            {
                account.Balance += amount;
                await _repository.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (DbUpdateException)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
        public async Task DecreaseBalanceAsync(Account account, decimal amount)
        {
            var transaction = await _repository.BeginTransactionAsync();
            try
            {
                if (amount > account.Balance)
                {
                    throw new InsufficientFundsException();
                }
                account.Balance -= amount;
                await _repository.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (InsufficientFundsException)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<long> GenerateAccountNumber()
        {
            long accountNumber;
            bool isUnique = false;
            do
            {
                Random random = new Random();
                string number = string.Concat(
                    random.Next(1, 10).ToString(),
                    random.Next(100000000, 999999999).ToString(),
                    random.Next(100000000, 999999999).ToString()
                );
                accountNumber = long.Parse(number);
                isUnique = await CheckIfAccountNumberIsUnique(accountNumber);
            } while (isUnique);
            return accountNumber;
        }
        private async Task<bool> CheckIfAccountNumberIsUnique(long accountNumber)
        {
            var accountNumbers = await _repository.GetAccountNumbersAsync();
            return accountNumbers.Contains(accountNumber);
        }
    }
}
