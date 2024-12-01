
using Payments.Repositories;

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
            string prefix = phoneNumber.Substring(2,3);
            long number = long.Parse(phoneNumber.Substring(2));
            var providerID = await _repository.GetPaymentProviderIdAsync(prefix, number);
            if (providerID == null)
            {
                throw new Exception();
            }
            Console.WriteLine($"ProviderID: {providerID}");
            return providerID;
        }
    }
}
