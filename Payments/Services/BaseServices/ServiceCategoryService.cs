using Microsoft.EntityFrameworkCore;
using Payments.DTOs;
using Payments.Models;
using Payments.Repositories;
using Payments.Services.Exceptions;

namespace Payments.Services.BaseServices
{
    public class ServiceCategoryService : IServiceCategoryService
    {
        private readonly IServiceCategoryRepository _repository;
        public ServiceCategoryService(IServiceCategoryRepository repository)
        {
            _repository = repository;
        }

        public async Task CreateServiceCategoryAsync(ServiceCategoryDTO serviceCategoryDTO)
        {
            try
            {
                var service_category = await _repository.SelectServiceCategoryByNameAsync(serviceCategoryDTO.Name);
                if (service_category != null)
                {
                    throw new ServiceCategoryAlreadyExistException();
                }
                var new_service_category = new ServiceCategory
                {
                    Name = serviceCategoryDTO.Name,
                    Description = serviceCategoryDTO.Description,
                };
                await _repository.AddServiceCategoryAsync(new_service_category);
            }
            catch (ServiceCategoryAlreadyExistException)
            {
                throw;
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

        public async Task EditServiceCategoryNameAsync(ChangeServiceCategoryNameDTO changeServiceCategoryNameDTO)
        {
            try
            {
                var service_category = await _repository.SelectServiceCategoryByNameAsync(changeServiceCategoryNameDTO.CurrentName);
                if (service_category == null)
                {
                    throw new ServiceCategoryNotFoundException();
                }
                service_category.Name = changeServiceCategoryNameDTO.NewName;
                await _repository.UpdateServiceCategoryAsync(service_category);
            }
            catch (ServiceCategoryNotFoundException)
            {
                throw;
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
        public async Task EditServiceCategoryDescriptionAsync(ChangeServiceCategoryDescriptionDTO changeServiceCategoryDescriptionDTO)
        {
            try
            {
                var service_category = await _repository.SelectServiceCategoryByNameAsync(changeServiceCategoryDescriptionDTO.Name);
                if (service_category == null)
                {
                    throw new ServiceCategoryNotFoundException();
                }
                service_category.Description = changeServiceCategoryDescriptionDTO.NewDescription;
                await _repository.UpdateServiceCategoryAsync(service_category);
            }
            catch (ServiceCategoryNotFoundException)
            {
                throw;
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
        public async Task RemoveServiceCategoryAsync(string serviceCategoryName)
        {
            try
            {
                var service_category = await _repository.SelectServiceCategoryByNameAsync(serviceCategoryName);
                if (service_category == null)
                {
                    throw new ServiceCategoryNotFoundException();
                }
                await _repository.DeleteServiceCategoryAsync(service_category);
            }
            catch (ServiceCategoryNotFoundException)
            {
                throw;
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
        public async Task<ServiceCategory?> GetServiceCategoryByNameAsync(string serviceCategoryName)
        {
            try
            {
                var service_category = await _repository.SelectServiceCategoryByNameAsync(serviceCategoryName);
                if (service_category == null)
                {
                    throw new ServiceCategoryNotFoundException();
                }
                return service_category;
            }
            catch (ServiceCategoryNotFoundException)
            {
                throw;
            }
        }
        public async Task<List<ServiceCategory>> GetAllServiceCategoryAsync()
        {
            return await _repository.SelectAllServiceCategoryAsync();
        }
    }
}
