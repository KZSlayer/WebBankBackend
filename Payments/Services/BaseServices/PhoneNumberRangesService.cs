using Microsoft.EntityFrameworkCore;
using Payments.DTOs.PhoneNumberRanges;
using Payments.Models;
using Payments.Repositories;
using Payments.Services.Exceptions;

namespace Payments.Services.BaseServices
{
    public class PhoneNumberRangesService : IPhoneNumberRangesService
    {
        private readonly IPhoneNumberRangesRepository _repository;
        public PhoneNumberRangesService(IPhoneNumberRangesRepository repository)
        {
            _repository = repository;
        }

        public async Task CreatePhoneNumberRangesAsync(PhoneNumberRangeDTO phoneNumberRangeDTO, int paymentProviderId)
        {
            try
            {
                var existPNR = await _repository.GetPhoneNumberRangesByPrefixAndPaymentProviderIdAsync(paymentProviderId, phoneNumberRangeDTO.Prefix);
                if (existPNR != null)
                {
                    throw new PhoneNumberRangeAlreadyExistException();
                }
                var newPNR = new PhoneNumberRange
                {
                    PaymentProviderId = paymentProviderId,
                    Prefix = phoneNumberRangeDTO.Prefix,
                    StartRange = phoneNumberRangeDTO.StartRange,
                    EndRange = phoneNumberRangeDTO.EndRange,
                };
                await _repository.AddPhoneNumberRangesAsync(newPNR);
            }
            catch (PhoneNumberRangeAlreadyExistException)
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

        public async Task EditPhoneNumberRangesPrefixAsync(EditPhoneNumberRangesPrefixDTO editPhoneNumberRangesPrefixDTO, int paymentProviderId)
        {
            try
            {
                var existPNR1 = await _repository.GetPhoneNumberRangesByPrefixAndPaymentProviderIdAsync(paymentProviderId, editPhoneNumberRangesPrefixDTO.CurrentPrefix);
                if (existPNR1 == null)
                {
                    throw new PhoneNumberRangeNotFoundException();
                }
                var existPNR2 = await _repository.GetPhoneNumberRangesByPrefixAndPaymentProviderIdAsync(paymentProviderId, editPhoneNumberRangesPrefixDTO.NewPrefix);
                if (existPNR2 != null)
                {
                    throw new PhoneNumberRangeAlreadyExistException();
                }
                existPNR1.Prefix = editPhoneNumberRangesPrefixDTO.NewPrefix;
                await _repository.UpdatePhoneNumberRangesAsync(existPNR1);
            }
            catch (PhoneNumberRangeNotFoundException)
            {
                throw;
            }
            catch (PhoneNumberRangeAlreadyExistException)
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
        public async Task EditPhoneNumberRangesStartRangesAsync(EditPhoneNumberRangesStartRangesDTO editPhoneNumberRangesStartRangesDTO, int paymentProviderId)
        {
            try
            {
                var existPNR = await _repository.GetPhoneNumberRangesByPrefixAndPaymentProviderIdAsync(paymentProviderId, editPhoneNumberRangesStartRangesDTO.Prefix);
                if (existPNR == null)
                {
                    throw new PhoneNumberRangeNotFoundException();
                }
                if (editPhoneNumberRangesStartRangesDTO.StartRange > existPNR.EndRange)
                {
                    throw new StartRangeGreaterThanEndRangeException();
                }
                existPNR.StartRange = editPhoneNumberRangesStartRangesDTO.StartRange;
                await _repository.UpdatePhoneNumberRangesAsync(existPNR);
            }
            catch (PhoneNumberRangeNotFoundException)
            {
                throw;
            }
            catch (StartRangeGreaterThanEndRangeException)
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
        public async Task EditPhoneNumberRangesEndRangesAsync(EditPhoneNumberRangesEndRangesDTO editPhoneNumberRangesEndRangesDTO, int paymentProviderId)
        {
            try
            {
                var existPNR = await _repository.GetPhoneNumberRangesByPrefixAndPaymentProviderIdAsync(paymentProviderId, editPhoneNumberRangesEndRangesDTO.Prefix);
                if (existPNR == null)
                {
                    throw new PhoneNumberRangeNotFoundException();
                }
                if (editPhoneNumberRangesEndRangesDTO.EndRange < existPNR.StartRange)
                {
                    throw new EndRangeLessThanStartRangeException();
                }
                existPNR.StartRange = editPhoneNumberRangesEndRangesDTO.EndRange;
                await _repository.UpdatePhoneNumberRangesAsync(existPNR);
            }
            catch (PhoneNumberRangeNotFoundException)
            {
                throw;
            }
            catch (EndRangeLessThanStartRangeException)
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


        public async Task DeletePhoneNumberRangesAsync(DeletePhoneNumberRangeDTO deletePhoneNumberRange, int paymentProviderId)
        {
            try
            {
                var existPNR = await _repository.GetPhoneNumberRangesByPrefixAndPaymentProviderIdAsync(paymentProviderId, deletePhoneNumberRange.Prefix);
                if (existPNR == null)
                {
                    throw new PhoneNumberRangeNotFoundException();
                }
                await _repository.RemovePhoneNumberRangesAsync(existPNR);
            }
            catch (PhoneNumberRangeNotFoundException)
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

        public async Task<int?> FindPaymentProviderIdAsync(string phoneNumber)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(phoneNumber))
                {
                    throw new ArgumentException();
                }
                string prefix = phoneNumber.Substring(2, 3);
                long number = long.Parse(phoneNumber.Substring(2));
                var ranges = await _repository.GetPhoneNumberRangesByPrefixAsync(prefix);
                var matchingRange = ranges.FirstOrDefault(range => number >= range.StartRange && number <= range.EndRange);
                if (matchingRange == null)
                {
                    throw new ProviderNotFoundException();
                }
                return matchingRange.PaymentProviderId;

            }
            catch (ProviderNotFoundException)
            {
                throw;
            }
            catch (ArgumentException)
            {
                throw;
            }
        }

        public Task<List<PhoneNumberRange>> GetAllPhoneNumberRangesAsync()
        {
            return _repository.GetAllPhoneNumberRangesAsync();
        }
    }
}
