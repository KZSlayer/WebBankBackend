using Payments.Repositories;
using Payments.Services.Exceptions;

namespace Payments.Services
{
    public class PhoneNumberRangesService : IPhoneNumberRangesService
    {
        private readonly IPhoneNumberRangesRepository _repository;
        public PhoneNumberRangesService(IPhoneNumberRangesRepository repository)
        {
            _repository = repository;
        }
        public async Task<int?> FindPaymentProviderIdAsync(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
            {
                throw new ArgumentException();
            }
            string prefix = phoneNumber.Substring(2,3);
            long number = long.Parse(phoneNumber.Substring(2));
            var ranges = await _repository.GetPhoneNumberRangesByPrefixAsync(prefix);
            var matchingRange = ranges.FirstOrDefault(range => number >= range.StartRange && number <= range.EndRange);
            if (matchingRange == null)
            {
                throw new ProviderNotFoundException();
            }
            return matchingRange.PaymentProviderId;
        }
    }
}
