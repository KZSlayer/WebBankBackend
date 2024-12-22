using Microsoft.EntityFrameworkCore.Storage;
using Payments.Models;

namespace Payments.Repositories
{
    public interface IPaymentTransactionRepository
    {
        Task AddPaymentTransactionAsync(PaymentTransaction transaction);
        Task EditPaymentTransactionStatusAsync(int transactionID, string status);
        Task<PaymentTransaction?> GetPaymentTransactionById(int transactionID);
        Task<IDbContextTransaction> BeginTransactionAsync();
    }
}
