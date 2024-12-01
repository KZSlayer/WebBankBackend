namespace Payments.Services
{
    public interface IPhoneNumberRangesService
    {
        Task<int?> FindPaymentProviderIdAsync(string phoneNumber);
    }
}
