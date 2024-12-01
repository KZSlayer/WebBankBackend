namespace Payments.Services
{
    public interface IPaymentProviderService
    {
        Task<int?> FindServiceCategoryIdAsync(int providerID);
    }
}
