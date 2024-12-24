using Payments.DTOs;
using Payments.Services.BaseServices;
using Payments.Services.Exceptions;
using System.Security.Claims;

namespace Payments.Services
{
    public class PayPhoneService : IPayPhoneService
    {
        private readonly IPhoneNumberRangesService _phoneNumberRangesService;
        private readonly IPaymentProviderService _paymentProviderService;
        private readonly IPaymentTransactionService _paymentTransactionService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public PayPhoneService(IPhoneNumberRangesService phoneNumberRangesService, IPaymentProviderService paymentProviderService, IPaymentTransactionService paymentTransactionService, IHttpContextAccessor httpContextAccessor)
        {
            _phoneNumberRangesService = phoneNumberRangesService;
            _paymentProviderService = paymentProviderService;
            _paymentTransactionService = paymentTransactionService;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task PayPhoneAsync(PayPhoneDTO payPhoneDTO)
        {
            try
            {
                var userId = _httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrWhiteSpace(userId))
                {
                    throw new UserNotAuthenticatedException();
                }
                var providerID = await _phoneNumberRangesService.FindPaymentProviderIdAsync(payPhoneDTO.PhoneNumber);
                var categoryID = await _paymentProviderService.FindServiceCategoryIdAsync(providerID.Value);
                await _paymentTransactionService.CreatePaymentTransactionAsync(int.Parse(userId), categoryID.Value, payPhoneDTO.Amount); // Проверка на Parse
            }
            catch (ProviderNotFoundException)
            {
                throw;
            }
            catch (ArgumentException)
            {
                throw;
            }
            catch (СategoryNotFoundException)
            {
                throw;
            }
            catch (PaymentTransactionAddException)
            {
                throw;
            }
            catch (UserNotAuthenticatedException)
            {
                throw;
            }
        } 
    }
}
