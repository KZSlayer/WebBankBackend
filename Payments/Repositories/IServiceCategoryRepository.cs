using Payments.Models;

namespace Payments.Repositories
{
    public interface IServiceCategoryRepository
    {
        Task AddServiceCategoryAsync(ServiceCategory serviceCategory);
        Task UpdateServiceCategoryAsync(ServiceCategory serviceCategory);
        Task DeleteServiceCategoryAsync(ServiceCategory serviceCategory);
        Task<ServiceCategory?> SelectServiceCategoryByNameAsync(string name);
        Task<List<ServiceCategory>> SelectAllServiceCategoryAsync();
    }
}
