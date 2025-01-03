﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Collections.Generic;
using Transaction.Data;
using Transaction.DTOs;
using Transaction.Models;

namespace Transaction.Repositories
{
    public class TransactionsRepository : ITransactionsRepository
    {
        private readonly TransactionDbContext _context;
        private readonly ILogger<TransactionsRepository> _logger;
        public TransactionsRepository(TransactionDbContext context, ILogger<TransactionsRepository> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task AddTransactionAsync(Transactions transaction)
        {
            try
            {
                await _context.transactions.AddAsync(transaction);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError($"Ошибка при добавлении транзакции в базу данных! Основная причина: {ex.InnerException?.Message}. Все детали: {ex}");
                throw;
            }
            catch (OperationCanceledException ex)
            {
                _logger.LogError($"Ошибка при добавлении транзакции в базу данных! Основная причина: {ex.InnerException?.Message}. Все детали: {ex}");
                throw;
            }
        }
        public Task<List<TransactionDTO>> SelectAllAccountTransactionsAsync(long accountNumber)
        {
            var transactions = _context.transactions
                .Where(t => t.FromAccountNumber == accountNumber || t.ToAccountNumber == accountNumber)
                .Include(t => t.TransactionType)
                .Select(t => new TransactionDTO
                {
                    Id = t.Id,
                    FromAccountNumber = t.FromAccountNumber,
                    ToAccountNumber = t.ToAccountNumber,
                    Amount = t.Amount,
                    TransactionType = t.TransactionType.Name,
                    Status = t.Status,
                    Timestamp = t.Timestamp,
                })
                .ToListAsync();
            return transactions;
        }
        public Task<List<TransactionDTO>> SelectAllTransactionsAsync()
        {
            var transactions = _context.transactions
                .Include(t => t.TransactionType)
                .Select(t => new TransactionDTO
                {
                    Id = t.Id,
                    FromAccountNumber = t.FromAccountNumber,
                    ToAccountNumber = t.ToAccountNumber,
                    Amount = t.Amount,
                    TransactionType = t.TransactionType.Name,
                    Status = t.Status,
                    Timestamp = t.Timestamp,
                })
                .ToListAsync();
            return transactions;
        }
        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await _context.Database.BeginTransactionAsync();
        }
    }
}
