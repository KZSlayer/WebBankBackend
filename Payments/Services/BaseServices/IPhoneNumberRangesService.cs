namespace Payments.Services.BaseServices
{
    public interface IPhoneNumberRangesService
    {
        Task<int?> FindPaymentProviderIdAsync(string phoneNumber);
    }
}
