
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Payments.Data;

namespace Payments.Repositories
{
    public class PhoneNumberRangesRepository : IPhoneNumberRangesRepository
    {
        private readonly PaymentsDbContext _context;
        public PhoneNumberRangesRepository(PaymentsDbContext context)
        {
            _context = context;
        }

        public async Task<int?> GetPaymentProviderIdAsync(string prefix, long number)
        {
            var providerID = await _context.phone_number_ranges
                .Where(pnr => pnr.Prefix == prefix && number >= pnr.StartRange && number <= pnr.EndRange)
                .Select(pnr => pnr.PaymentProviderId)
                .FirstOrDefaultAsync();
            return providerID;
        }
        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await _context.Database.BeginTransactionAsync();
        }
    }
}
