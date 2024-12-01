using Microsoft.EntityFrameworkCore.Storage;
using Payments.Models;

namespace Payments.Repositories
{
    public interface IPaymentTransactionRepository
    {
        Task AddPaymentTransactionAsync(PaymentTransaction transaction);
        Task<IDbContextTransaction> BeginTransactionAsync();
    }
}
