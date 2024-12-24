using Microsoft.EntityFrameworkCore;
using Payments.Data;
using Payments.DTOs;
using Payments.Models;

namespace Payments.Repositories
{
    public class ServiceCategoryRepository : IServiceCategoryRepository
    {
        private readonly PaymentsDbContext _context;
        public ServiceCategoryRepository(PaymentsDbContext context)
        {
            _context = context;
        }

        public async Task AddServiceCategoryAsync(ServiceCategory serviceCategory)
        {
            try
            {
                await _context.service_categories.AddAsync(serviceCategory);
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
        public async Task UpdateServiceCategoryAsync(ServiceCategory serviceCategory)
        {
            try
            {
                 _context.service_categories.Update(serviceCategory);
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
        public async Task DeleteServiceCategoryAsync(ServiceCategory serviceCategory)
        {
            try
            {
                _context.service_categories.Remove(serviceCategory);
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
        public async Task<ServiceCategory?> SelectServiceCategoryByNameAsync(string serviceCategoryName)
        {
            return await _context.service_categories.FirstOrDefaultAsync(sc => sc.Name == serviceCategoryName);
        }
        public async Task<List<ServiceCategory>> SelectAllServiceCategoryAsync()
        {
            return await _context.service_categories.ToListAsync();
        }
    }
}
