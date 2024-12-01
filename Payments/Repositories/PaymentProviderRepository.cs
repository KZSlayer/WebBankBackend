
using Microsoft.EntityFrameworkCore;
using Payments.Data;

namespace Payments.Repositories
{
    public class PaymentProviderRepository : IPaymentProviderRepository
    {
        private readonly PaymentsDbContext _context;
        public PaymentProviderRepository(PaymentsDbContext context)
        {
            _context = context;
        }
        public async Task<int?> GetServiceCategoryIdAsync(int providerID)
        {
            var categoryId = await _context.payment_providers
                .Where(pp => pp.Id == providerID)
                .Select(pp => pp.ServiceCategoryId)
                .FirstOrDefaultAsync();
            return categoryId;
        }
    }
}
