﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Payments.Data;
using Payments.Models;
using Payments.Services.Exceptions;

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
            try
            {
                await _context.payment_transactions.AddAsync(transaction);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                throw new PaymentTransactionAddException("Ошибка при добавлении транзакции в базу данных.");
            }
            catch (OperationCanceledException)
            {
                throw new PaymentTransactionAddException("Операция добавления транзакции была прервана.");
            }
        }
        public async Task<PaymentTransaction?> GetPaymentTransactionById(int transactionID)
        {
            return await _context.payment_transactions.FirstOrDefaultAsync(pt => pt.Id == transactionID);
        }
        public async Task EditPaymentTransactionStatusAsync(int transactionID, string status)
        {
            try
            {
                var transaction = await _context.payment_transactions.FirstOrDefaultAsync(pt => pt.Id == transactionID);
                transaction.Status = status;
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                throw new PaymentTransactionUpdateException("Ошибка при обновлении статуса транзакции.");
            }
            catch (OperationCanceledException)
            {
                throw new PaymentTransactionUpdateException("Операция обновления статуса транзакции была прервана.");
            }
            
        }
        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await _context.Database.BeginTransactionAsync();
        }
    }
}
