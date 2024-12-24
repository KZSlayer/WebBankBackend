using Microsoft.EntityFrameworkCore;
using Payments.DTOs.PaymentProviderDTOs;
using Payments.Models;
using Payments.Repositories;
using Payments.Services.Exceptions;

namespace Payments.Services.BaseServices
{
    public class PaymentProviderService : IPaymentProviderService
    {
        private readonly IPaymentProviderRepository _repository;
        public PaymentProviderService(IPaymentProviderRepository repository)
        {
            _repository = repository;
        }
        public async Task CreatePaymentProviderAsync(PaymentProviderDTO paymentProviderDTO, int serviceCategoryId)
        {
            try
            {
                var payment_provider = await _repository.SelectPaymentProviderByNameAsync(paymentProviderDTO.Name);
                if (payment_provider != null)
                {
                    throw new PaymentProviderAlreadyExistException();
                }
                var new_payment_provider = new PaymentProvider
                {
                    Name = paymentProviderDTO.Name,
                    Description = paymentProviderDTO.Description,
                    ServiceCategoryId = serviceCategoryId,
                };
                await _repository.AddPaymentProviderAsync(new_payment_provider);
            }
            catch (PaymentProviderAlreadyExistException)
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

        public async Task EditPaymentProviderNameAsync(ChangePaymentProviderNameDTO changePaymentProviderNameDTO)
        {
            try
            {
                var payment_provider = await _repository.SelectPaymentProviderByNameAsync(changePaymentProviderNameDTO.CurrentName);
                if (payment_provider == null)
                {
                    throw new PaymentProviderNotFoundException();
                }
                payment_provider.Name = changePaymentProviderNameDTO.NewName;
                await _repository.UpdatePaymentProviderAsync(payment_provider);
            }
            catch (PaymentProviderNotFoundException)
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
        public async Task EditPaymentProviderDescriptionAsync(ChangePaymentProviderDescriptionDTO changePaymentProviderDescriptionDTO)
        {
            try
            {
                var payment_provider = await _repository.SelectPaymentProviderByNameAsync(changePaymentProviderDescriptionDTO.Name);
                if (payment_provider == null)
                {
                    throw new PaymentProviderNotFoundException();
                }
                payment_provider.Description = changePaymentProviderDescriptionDTO.NewDescription;
                await _repository.UpdatePaymentProviderAsync(payment_provider);
            }
            catch (PaymentProviderNotFoundException)
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
        public async Task EditPaymentProviderServiceCategoryIdAsync(ChangePaymentProviderServiceCategoryIdDTO changePaymentProviderServiceCategoryIdDTO, int serviceCategoryId)
        {
            try
            {
                var payment_provider = await _repository.SelectPaymentProviderByNameAsync(changePaymentProviderServiceCategoryIdDTO.Name);
                if (payment_provider == null)
                {
                    throw new PaymentProviderNotFoundException();
                }
                payment_provider.ServiceCategoryId = serviceCategoryId;
                await _repository.UpdatePaymentProviderAsync(payment_provider);
            }
            catch (PaymentProviderNotFoundException)
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
        public async Task RemovePaymentProviderAsync(string paymentProviderName)
        {
            try
            {
                var service_category = await _repository.SelectPaymentProviderByNameAsync(paymentProviderName);
                if (service_category == null)
                {
                    throw new PaymentProviderNotFoundException();
                }
                await _repository.DeletePaymentProviderAsync(service_category);
            }
            catch (PaymentProviderNotFoundException)
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

        public async Task<List<PaymentProvider>> GetAllPaymentProviderAsync()
        {
            return await _repository.SelectAllPaymentProviderAsync();
        }
        public async Task<PaymentProvider?> FindPaymentProviderByNameAsync(string name)
        {
            try
            {
                var paymentProvider = await _repository.SelectPaymentProviderByNameAsync(name);
                if (paymentProvider == null)
                {
                    throw new PaymentProviderNotFoundException();
                }
                return paymentProvider;
            }
            catch (PaymentProviderNotFoundException)
            {
                throw;
            }
        }
        public async Task<int?> FindServiceCategoryIdAsync(int providerID)
        {
            try
            {
                var categoryId = await _repository.GetServiceCategoryIdAsync(providerID);
                if (categoryId == null)
                {
                    throw new СategoryNotFoundException();
                }
                return categoryId;
            }
            catch (СategoryNotFoundException)
            {
                throw;
            }
        }
    }
}
