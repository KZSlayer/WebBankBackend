using Microsoft.EntityFrameworkCore;
using Payments.Data;
using Payments.Models;

namespace Payments.Repositories
{
    public class PaymentProviderRepository : IPaymentProviderRepository
    {
        private readonly PaymentsDbContext _context;
        public PaymentProviderRepository(PaymentsDbContext context)
        {
            _context = context;
        }
        public async Task AddPaymentProviderAsync(PaymentProvider paymentProvider)
        {
            try
            {
                await _context.payment_providers.AddAsync(paymentProvider);
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
        public async Task UpdatePaymentProviderAsync(PaymentProvider paymentProvider)
        {
            try
            {
                _context.payment_providers.Update(paymentProvider);
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
        public async Task DeletePaymentProviderAsync(PaymentProvider paymentProvider)
        {
            try
            {
                _context.payment_providers.Remove(paymentProvider);
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
        public async Task<PaymentProvider?> SelectPaymentProviderByNameAsync(string name)
        {
            return await _context.payment_providers.FirstOrDefaultAsync(pp => pp.Name == name);
        }
        public async Task<List<PaymentProvider>> SelectAllPaymentProviderAsync()
        {
            return await _context.payment_providers.ToListAsync();
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
