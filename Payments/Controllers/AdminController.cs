using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Payments.DTOs.PaymentProviderDTOs;
using Payments.DTOs.PhoneNumberRanges;
using Payments.DTOs.ServiceCategoryDTOs;
using Payments.Filters;
using Payments.Services.BaseServices;

namespace Payments.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [ServiceFilter(typeof(CustomExceptionFilter))]
    public class AdminController : ControllerBase
    {
        private readonly IServiceCategoryService _serviceCategoryService;
        private readonly IPaymentProviderService _paymentProviderService;
        private readonly IPhoneNumberRangesService _phoneNumberRangesService;
        private readonly IPaymentTransactionService _paymentTransactionService;
        public AdminController(IServiceCategoryService serviceCategoryService, IPaymentProviderService paymentProviderService, IPhoneNumberRangesService phoneNumberRangesService, IPaymentTransactionService paymentTransactionService)
        {
            _serviceCategoryService = serviceCategoryService;
            _paymentProviderService = paymentProviderService;
            _phoneNumberRangesService = phoneNumberRangesService;
            _paymentTransactionService = paymentTransactionService;
        }

        [HttpPost("AddServiceCategory")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> AddServiceCategory([FromBody] ServiceCategoryDTO serviceCategoryDTO)
        {
            await _serviceCategoryService.CreateServiceCategoryAsync(serviceCategoryDTO);
            return Ok();
        }

        [HttpPatch("ChangeServiceCategoryName")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> ChangeServiceCategoryName([FromBody] ChangeServiceCategoryNameDTO changeServiceCategoryNameDTO)
        {
            await _serviceCategoryService.EditServiceCategoryNameAsync(changeServiceCategoryNameDTO);
            return Ok();
        }

        [HttpPatch("ChangeServiceCategoryDescription")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> ChangeServiceCategoryDescription([FromBody] ChangeServiceCategoryDescriptionDTO changeServiceCategoryDescriptionDTO)
        {
            await _serviceCategoryService.EditServiceCategoryDescriptionAsync(changeServiceCategoryDescriptionDTO);
            return Ok();
        }

        [HttpDelete("DeleteServiceCategory")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteServiceCategory(string name)
        {
            await _serviceCategoryService.RemoveServiceCategoryAsync(name);
            return Ok();
        }

        [HttpGet("GetAllServiceCategory")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetAllServiceCategory()
        {
            var serviceCategories = await _serviceCategoryService.GetAllServiceCategoryAsync();
            return Ok(serviceCategories);
        }

        [HttpPost("AddPaymentProvider")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> AddPaymentProvider([FromBody] PaymentProviderDTO paymentProviderDTO)
        {
            var service_category = await _serviceCategoryService.GetServiceCategoryByNameAsync(paymentProviderDTO.Name);
            await _paymentProviderService.CreatePaymentProviderAsync(paymentProviderDTO, service_category.Id);
            return Ok();
        }

        [HttpPatch("ChangePaymentProviderName")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> ChangePaymentProviderName([FromBody] ChangePaymentProviderNameDTO changePaymentProviderNameDTO)
        {
            await _paymentProviderService.EditPaymentProviderNameAsync(changePaymentProviderNameDTO);
            return Ok();
        }

        [HttpPatch("ChangePaymentProviderDescription")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> ChangePaymentProviderDescription([FromBody] ChangePaymentProviderDescriptionDTO changePaymentProviderDescriptionDTO)
        {
            await _paymentProviderService.EditPaymentProviderDescriptionAsync(changePaymentProviderDescriptionDTO);
            return Ok();
        }

        [HttpPatch("ChangePaymentProviderServiceCategoryId")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> ChangePaymentProviderServiceCategoryId([FromBody] ChangePaymentProviderServiceCategoryIdDTO changePaymentProviderServiceCategoryIdDTO)
        {
            var service_category = await _serviceCategoryService.GetServiceCategoryByNameAsync(changePaymentProviderServiceCategoryIdDTO.Name);
            await _paymentProviderService.EditPaymentProviderServiceCategoryIdAsync(changePaymentProviderServiceCategoryIdDTO, service_category.Id);
            return Ok();
        }

        [HttpDelete("DeletePaymentProvider")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeletePaymentProvider(string name)
        {
            await _paymentProviderService.RemovePaymentProviderAsync(name);
            return Ok();
        }

        [HttpGet("GetAllPaymentProvider")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetAllPaymentProvider()
        {
            var serviceCategories = await _paymentProviderService.GetAllPaymentProviderAsync();
            return Ok(serviceCategories);
        }

        [HttpPost("AddPhoneNumberRanges")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> AddPhoneNumberRanges([FromBody] PhoneNumberRangeDTO phoneNumberRangeDTO)
        {
            var paymentProvider = await _paymentProviderService.FindPaymentProviderByNameAsync(phoneNumberRangeDTO.PaymentProviderName);
            await _phoneNumberRangesService.CreatePhoneNumberRangesAsync(phoneNumberRangeDTO, paymentProvider.Id);
            return Ok();
        }

        [HttpPatch("ChangePhoneNumberRangesPrefix")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> ChangePhoneNumberRangesPrefix([FromBody] EditPhoneNumberRangesPrefixDTO editPhoneNumberRangesPrefixDTO)
        {
            var paymentProvider = await _paymentProviderService.FindPaymentProviderByNameAsync(editPhoneNumberRangesPrefixDTO.PaymentProviderName);
            await _phoneNumberRangesService.EditPhoneNumberRangesPrefixAsync(editPhoneNumberRangesPrefixDTO, paymentProvider.Id);
            return Ok();
        }

        [HttpPatch("ChangePhoneNumberRangesStartRanges")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> ChangePhoneNumberRangesStartRanges([FromBody] EditPhoneNumberRangesStartRangesDTO editPhoneNumberRangesStartRangesDTO)
        {
            var paymentProvider = await _paymentProviderService.FindPaymentProviderByNameAsync(editPhoneNumberRangesStartRangesDTO.PaymentProviderName);
            await _phoneNumberRangesService.EditPhoneNumberRangesStartRangesAsync(editPhoneNumberRangesStartRangesDTO, paymentProvider.Id);
            return Ok();
        }

        [HttpPatch("ChangePhoneNumberRangesEndRanges")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> ChangePhoneNumberRangesEndRanges([FromBody] EditPhoneNumberRangesEndRangesDTO editPhoneNumberRangesEndRangesDTO)
        {
            var paymentProvider = await _paymentProviderService.FindPaymentProviderByNameAsync(editPhoneNumberRangesEndRangesDTO.PaymentProviderName);
            await _phoneNumberRangesService.EditPhoneNumberRangesEndRangesAsync(editPhoneNumberRangesEndRangesDTO, paymentProvider.Id);
            return Ok();
        }

        [HttpDelete("DeletePhoneNumberRanges")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeletePhoneNumberRanges([FromBody] DeletePhoneNumberRangeDTO deletePhoneNumberRangeDTO)
        {
            var paymentProvider = await _paymentProviderService.FindPaymentProviderByNameAsync(deletePhoneNumberRangeDTO.PaymentProviderName);
            await _phoneNumberRangesService.DeletePhoneNumberRangesAsync(deletePhoneNumberRangeDTO, paymentProvider.Id);
            return Ok();
        }

        [HttpGet("GetAllPhoneNumberRanges")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetAllPhoneNumberRanges()
        {
            var phoneNumberRanges = await _phoneNumberRangesService.GetAllPhoneNumberRangesAsync();
            return Ok(phoneNumberRanges);
        }

        [HttpGet("GetAllPaymentTransaction")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetAllPaymentTransaction()
        {
            var paymentTransactions = await _paymentTransactionService.GetAllPaymentTransactionsAsync();
            return Ok(paymentTransactions);
        }
    }
}
