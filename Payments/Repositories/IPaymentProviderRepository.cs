using Payments.Models;

namespace Payments.Repositories
{
    public interface IPaymentProviderRepository
    {
        Task AddPaymentProviderAsync(PaymentProvider paymentProvider);
        Task UpdatePaymentProviderAsync(PaymentProvider paymentProvider);
        Task DeletePaymentProviderAsync(PaymentProvider paymentProvider);
        Task<PaymentProvider?> SelectPaymentProviderByNameAsync(string name);
        Task<List<PaymentProvider>> SelectAllPaymentProviderAsync();
        Task<int?> GetServiceCategoryIdAsync(int providerID);
    }
}
