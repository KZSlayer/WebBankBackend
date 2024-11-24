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
    }
}
