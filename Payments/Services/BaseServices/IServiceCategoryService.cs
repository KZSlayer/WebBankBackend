using Payments.DTOs.ServiceCategoryDTOs;
using Payments.Models;

namespace Payments.Services.BaseServices
{
    public interface IServiceCategoryService
    {
        Task CreateServiceCategoryAsync(ServiceCategoryDTO serviceCategoryDTO);
        Task EditServiceCategoryNameAsync(ChangeServiceCategoryNameDTO changeServiceCategoryNameDTO);
        Task EditServiceCategoryDescriptionAsync(ChangeServiceCategoryDescriptionDTO changeServiceCategoryDescriptionDTO);
        Task RemoveServiceCategoryAsync(string serviceCategoryName);
        Task<ServiceCategory?> GetServiceCategoryByNameAsync(string serviceCategoryName);
        Task<List<ServiceCategory>> GetAllServiceCategoryAsync();
    }
}