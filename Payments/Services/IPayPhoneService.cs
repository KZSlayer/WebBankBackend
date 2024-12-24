using Payments.DTOs;

namespace Payments.Services
{
    public interface IPayPhoneService
    {
        Task PayPhoneAsync(PayPhoneDTO payPhoneDTO);
    }
}
