using Payments.DTOs;
using Payments.Models;

namespace Payments.Services.BaseServices
{
    public interface IPaymentProviderService
    {
        Task CreatePaymentProviderAsync(PaymentProviderDTO paymentProviderDTO, int serviceCategoryId);
        Task EditPaymentProviderNameAsync(ChangePaymentProviderNameDTO changePaymentProviderNameDTO);
        Task EditPaymentProviderDescriptionAsync(ChangePaymentProviderDescriptionDTO changePaymentProviderDescriptionDTO);
        Task EditPaymentProviderServiceCategoryIdAsync(ChangePaymentProviderServiceCategoryIdDTO changePaymentProviderServiceCategoryIdDTO, int serviceCategoryId);
        Task RemovePaymentProviderAsync(string paymentProviderName);
        Task<List<PaymentProvider>> GetAllPaymentProviderAsync();
        Task<int?> FindServiceCategoryIdAsync(int providerID);
    }
}
