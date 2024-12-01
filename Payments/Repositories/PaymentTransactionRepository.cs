using Microsoft.EntityFrameworkCore.Storage;
using Payments.Data;
using Payments.Models;

namespace Payments.Repositories
{
    public class PaymentTransactionRepository : IPaymentTransactionRepository
    {
        private readonly PaymentsDbContext _context;
        public PaymentTransactionRepository(PaymentsDbContext context)
        {
            _context = context;
        }

        public async Task AddPaymentTransactionAsync(PaymentTransaction transaction)
        {
            await _context.payment_transactions.AddAsync(transaction);
            await _context.SaveChangesAsync();
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await _context.Database.BeginTransactionAsync();
        }
    }
}
