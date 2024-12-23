using Payments.Repositories;
using Payments.Services.Exceptions;

namespace Payments.Services
{
    public class PaymentProviderService : IPaymentProviderService
    {
        private readonly IPaymentProviderRepository _repository;
        public PaymentProviderService(IPaymentProviderRepository repository)
        {
            _repository = repository;
        }
        public async Task<int?> FindServiceCategoryIdAsync(int providerID)
        {
            try
            {
                var categoryId = await _repository.GetServiceCategoryIdAsync(providerID);
                if (categoryId == null)
                {
                    throw new СategoryNotFoundException();
                }
                return categoryId;
            }
            catch (СategoryNotFoundException)
            {
                throw;
            }
        }
    }
}
