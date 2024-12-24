using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Payments.Data;
using Payments.Models;

namespace Payments.Repositories
{
    public class PhoneNumberRangesRepository : IPhoneNumberRangesRepository
    {
        private readonly PaymentsDbContext _context;
        private readonly ILogger<PhoneNumberRangesRepository> _logger;
        public PhoneNumberRangesRepository(PaymentsDbContext context, ILogger<PhoneNumberRangesRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task AddPhoneNumberRangesAsync(PhoneNumberRange phoneNumberRange)
        {
            try
            {
                await _context.phone_number_ranges.AddAsync(phoneNumberRange);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError($"Ошибка при добавлении диапазона номеров! Основная причина: {ex.InnerException?.Message}. Все детали: {ex}");
                throw;
            }
            catch (OperationCanceledException ex)
            {
                _logger.LogError($"Ошибка при добавлении диапазона номеров! Основная причина: {ex.InnerException?.Message}. Все детали: {ex}");
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
            catch (DbUpdateException ex)
            {
                _logger.LogError($"Ошибка при обновлении диапазона номеров! Основная причина: {ex.InnerException?.Message}. Все детали: {ex}");
                throw;
            }
            catch (OperationCanceledException ex)
            {
                _logger.LogError($"Ошибка при обновлении диапазона номеров! Основная причина: {ex.InnerException?.Message}. Все детали: {ex}");
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
            catch (DbUpdateException ex)
            {
                _logger.LogError($"Ошибка при удалении диапазона номеров! Основная причина: {ex.InnerException?.Message}. Все детали: {ex}");
                throw;
            }
            catch (OperationCanceledException ex)
            {
                _logger.LogError($"Ошибка при удалении диапазона номеров! Основная причина: {ex.InnerException?.Message}. Все детали: {ex}");
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
