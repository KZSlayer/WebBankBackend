using Microsoft.EntityFrameworkCore.Storage;
using Payments.Models;

namespace Payments.Repositories
{
    public interface IPaymentTransactionRepository
    {
        Task AddPaymentTransactionAsync(PaymentTransaction transaction);
        Task UpdatePaymentTransactionAsync(PaymentTransaction transaction);
        Task<PaymentTransaction?> GetPaymentTransactionById(int transactionID);
        Task<List<PaymentTransaction>> SelectAllPaymentTransactionsAsync();
        Task<List<PaymentTransaction>> SelectAllUserPaymentTransactionsAsync(int userId);
        Task<IDbContextTransaction> BeginTransactionAsync();
    }
}
