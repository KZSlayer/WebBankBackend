using Payments.DTOs.PhoneNumberRanges;
using Payments.Models;

namespace Payments.Services.BaseServices
{
    public interface IPhoneNumberRangesService
    {
        Task CreatePhoneNumberRangesAsync(PhoneNumberRangeDTO phoneNumberRangeDTO, int paymentProviderId);
        Task EditPhoneNumberRangesPrefixAsync(EditPhoneNumberRangesPrefixDTO editPhoneNumberRangesPrefixDTO, int paymentProviderId);
        Task EditPhoneNumberRangesStartRangesAsync(EditPhoneNumberRangesStartRangesDTO editPhoneNumberRangesStartRangesDTO, int paymentProviderId);
        Task EditPhoneNumberRangesEndRangesAsync(EditPhoneNumberRangesEndRangesDTO editPhoneNumberRangesEndRangesDTO, int paymentProviderId);
        Task DeletePhoneNumberRangesAsync(DeletePhoneNumberRangeDTO deletePhoneNumberRange, int paymentProviderId);
        Task<List<PhoneNumberRange>> GetAllPhoneNumberRangesAsync();
        Task<int?> FindPaymentProviderIdAsync(string phoneNumber);
    }
}
