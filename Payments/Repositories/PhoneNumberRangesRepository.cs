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

        public async Task AddPhoneNumberRangesAsync(PhoneNumberRange phoneNumberRange)
        {
            try
            {
                await _context.phone_number_ranges.AddAsync(phoneNumberRange);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                throw;
            }
            catch (OperationCanceledException)
            {
                throw;
            }
        }

        public async Task UpdatePhoneNumberRangesAsync(PhoneNumberRange phoneNumberRange)
        {
            try
            {
                _context.phone_number_ranges.Update(phoneNumberRange);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                throw;
            }
            catch (OperationCanceledException)
            {
                throw;
            }
        }

        public async Task RemovePhoneNumberRangesAsync(PhoneNumberRange phoneNumberRange)
        {
            try
            {
                _context.phone_number_ranges.Remove(phoneNumberRange);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                throw;
            }
            catch (OperationCanceledException)
            {
                throw;
            }
        }

        public Task<List<PhoneNumberRange>> GetAllPhoneNumberRangesAsync()
        {
            return _context.phone_number_ranges.ToListAsync();
        }

        public async Task<List<PhoneNumberRange>> GetPhoneNumberRangesByPrefixAsync(string prefix)
        {
            return await _context.phone_number_ranges
                .Where(pnr => pnr.Prefix == prefix)
                .ToListAsync();
        }

        public async Task<PhoneNumberRange?> GetPhoneNumberRangesByPrefixAndPaymentProviderIdAsync(int paymentProviderId, string prefix)
        {
            return await _context.phone_number_ranges.FirstOrDefaultAsync(pnr => pnr.PaymentProviderId == paymentProviderId && pnr.Prefix == prefix);
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await _context.Database.BeginTransactionAsync();
        }
    }
}
