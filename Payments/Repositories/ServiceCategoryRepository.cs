using Microsoft.EntityFrameworkCore;
using Payments.Data;
using Payments.DTOs;
using Payments.Models;

namespace Payments.Repositories
{
    public class ServiceCategoryRepository : IServiceCategoryRepository
    {
        private readonly PaymentsDbContext _context;
        private readonly ILogger<ServiceCategoryRepository> _logger;
        public ServiceCategoryRepository(PaymentsDbContext context, ILogger<ServiceCategoryRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task AddServiceCategoryAsync(ServiceCategory serviceCategory)
        {
            try
            {
                await _context.service_categories.AddAsync(serviceCategory);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError($"Ошибка при добавлении категории! Детали: {ex}");
                throw;
            }
            catch (OperationCanceledException ex)
            {
                _logger.LogError($"Ошибка при добавлении категории! Детали: {ex}");
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
            catch (DbUpdateException ex)
            {
                _logger.LogError($"Ошибка при обновлении категории! Детали: {ex}");
                throw;
            }
            catch (OperationCanceledException ex)
            {
                _logger.LogError($"Ошибка при обновлении категории! Детали: {ex}");
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
            catch (DbUpdateException ex)
            {
                _logger.LogError($"Ошибка при удалении категории! Детали: {ex}");
                throw;
            }
            catch (OperationCanceledException ex)
            {
                _logger.LogError($"Ошибка при удалении категории! Детали: {ex}");
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
