using Microsoft.EntityFrameworkCore;
using Transaction.Models;
using Transaction.Repositories;

namespace Transaction.Services
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
        public async Task FindByIdAsync(int userID)
        {
            try
            {
                var _account = await _repository.FindByIdAsync(userID);
                if (_account == null)
                {
                    throw new InvalidDataException("Аккаунт не найден!");
                }
            }
            catch (InvalidDataException)
            {
                throw new InvalidDataException("Аккаунт не найден!");
            }
            
        }
        public async Task IncreaseBalanceAsync(int userID, decimal amount)
        {
            var transaction = await _repository.BeginTransactionAsync();
            try
            {
                var _account = await _repository.FindByIdAsync(userID);
                if (_account == null)
                {
                    throw new InvalidDataException("Аккаунт не найден!");
                }
                _account.Balance += amount;
                await _repository.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (InvalidDataException)
            {
                await transaction.RollbackAsync();
                throw new InvalidDataException("Аккаунт не найден!");
            }
            catch (DbUpdateException)
            {
                await transaction.RollbackAsync();
                Console.WriteLine("IncreaseBalanceAsync");
                throw new DbUpdateException("Данные не сохранились");
            }
        }
        public async Task DecreaseBalanceAsync(int userID, decimal amount)
        {
            var transaction = await _repository.BeginTransactionAsync();
            try
            {
                var _account = await _repository.FindByIdAsync(userID);
                if (_account == null)
                {
                    throw new Exception("Аккаунт не найден!");
                }
                if (amount > _account.Balance)
                {
                    throw new Exception("Недостаточно денег на счёте!");
                }
                _account.Balance -= amount;
                await _repository.SaveChangesAsync();
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
