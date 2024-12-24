using Microsoft.EntityFrameworkCore.Storage;
using Payments.Models;

namespace Payments.Repositories
{
    public interface IPhoneNumberRangesRepository
    {
        Task AddPhoneNumberRangesAsync(PhoneNumberRange phoneNumberRange);
        Task UpdatePhoneNumberRangesAsync(PhoneNumberRange phoneNumberRange);
        Task RemovePhoneNumberRangesAsync(PhoneNumberRange phoneNumberRange);
        Task<List<PhoneNumberRange>> GetAllPhoneNumberRangesAsync();
        Task<List<PhoneNumberRange>> GetPhoneNumberRangesByPrefixAsync(string prefix);
        Task<PhoneNumberRange?> GetPhoneNumberRangesByPrefixAndPaymentProviderIdAsync(int paymentProviderId, string prefix);
        Task<IDbContextTransaction> BeginTransactionAsync();
    }
}
