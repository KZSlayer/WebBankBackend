
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Payments.Data;
using Payments.Models;

namespace Payments.Repositories
{
    public class PhoneNumberRangesRepository : IPhoneNumberRangesRepository
    {
        private readonly PaymentsDbContext _context;
        public PhoneNumberRangesRepository(PaymentsDbContext context)
        {
            _context = context;
        }

        public async Task<List<PhoneNumberRange>> GetPhoneNumberRangesByPrefixAsync(string prefix)
        {
            return await _context.phone_number_ranges
                .Where(pnr => pnr.Prefix == prefix)
                .ToListAsync();
        }
        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await _context.Database.BeginTransactionAsync();
        }
    }
}
