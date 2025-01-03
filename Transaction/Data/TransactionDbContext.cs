﻿using Microsoft.EntityFrameworkCore;
using Transaction.Models;

namespace Transaction.Data
{
    public class TransactionDbContext : DbContext
    {
        public DbSet<Account> accounts { get; set; }
        public DbSet<Transactions> transactions { get; set; }
        public DbSet<TransactionType> transaction_types { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=WebBank_TransactionDB;Username=postgres;Password=nvidia123");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TransactionType>().HasData(
                new TransactionType
                {
                    Id = 1,
                    Name = "перевод",
                    Description = "Перевод средств между счетами"
                },
                new TransactionType
                {
                    Id = 2,
                    Name = "пополнение",
                    Description = "Пополнение счета"
                },
                new TransactionType
                {
                    Id = 3,
                    Name = "списание",
                    Description = "Списание средств со счета"
                }
            );
            modelBuilder.Entity<Transactions>()
                .HasOne<Account>()
                .WithMany()
                .HasForeignKey(t => t.FromAccountNumber)
                .HasPrincipalKey(a => a.AccountNumber)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Transactions>()
                .HasOne<Account>()
                .WithMany()
                .HasForeignKey(t => t.ToAccountNumber)
                .HasPrincipalKey(a => a.AccountNumber)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Transactions>()
                .HasOne(t => t.TransactionType)
                .WithMany()
                .HasForeignKey(t => t.TransactionTypeId);
        }
    }
}
