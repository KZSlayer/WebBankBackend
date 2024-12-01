namespace Payments.Repositories
{
    public interface IPaymentProviderRepository
    {
        Task<int?> GetServiceCategoryIdAsync(int providerID);
    }
}
