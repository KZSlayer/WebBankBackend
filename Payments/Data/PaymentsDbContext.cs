using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Payments.Models;
using System.Collections.Generic;

namespace Payments.Data
{
    public class PaymentsDbContext : DbContext
    {
        public DbSet<PaymentTransaction> payment_transactions { get; set; }
        public DbSet<PaymentProvider> payment_providers { get; set; }
        public DbSet<ServiceCategory> service_categories { get; set; }
        public DbSet<PhoneNumberRange> phone_number_ranges { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=WebBank_PaymentsDB;Username=postgres;Password=nvidia123");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<PaymentTransaction>().ToTable("payment_transactions");
            modelBuilder.Entity<PaymentProvider>().ToTable("payment_providers");
            modelBuilder.Entity<ServiceCategory>().ToTable("service_categories");
            modelBuilder.Entity<PhoneNumberRange>().ToTable("phone_number_ranges");

            modelBuilder.Entity<PaymentTransaction>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Amount)
                      .HasColumnType("decimal(18,2)")
                      .IsRequired();
                entity.Property(e => e.Currency)
                      .HasMaxLength(3)
                      .IsRequired();
                entity.Property(e => e.Status)
                      .HasMaxLength(64)
                      .IsRequired();
                entity.Property(e => e.Timestamp)
                      .HasDefaultValueSql("CURRENT_TIMESTAMP");
                entity.HasOne(e => e.ServiceCategory)
                      .WithMany(sc => sc.PaymentTransactions)
                      .HasForeignKey(e => e.ServiceCategoryId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<PaymentProvider>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name)
                      .HasMaxLength(100)
                      .IsRequired();
                entity.Property(e => e.Description)
                      .HasMaxLength(255);
                entity.HasMany(e => e.ServiceCategories)
                      .WithOne(sc => sc.PaymentProvider)
                      .HasForeignKey(sc => sc.PaymentProviderId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<ServiceCategory>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name)
                      .HasMaxLength(100)
                      .IsRequired();
                entity.Property(e => e.Description)
                      .HasMaxLength(255);
                entity.HasOne(e => e.PaymentProvider)
                      .WithMany(p => p.ServiceCategories)
                      .HasForeignKey(e => e.PaymentProviderId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<PhoneNumberRange>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Prefix)
                      .HasMaxLength(3)
                      .IsRequired();
                entity.Property(e => e.StartRange)
                      .IsRequired();
                entity.Property(e => e.EndRange)
                      .IsRequired();
                entity.HasOne(e => e.PaymentProvider)
                      .WithMany()
                      .HasForeignKey(e => e.PaymentProviderId)
                      .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
