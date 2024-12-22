using Microsoft.EntityFrameworkCore.Storage;
using Payments.Models;

namespace Payments.Repositories
{
    public interface IPhoneNumberRangesRepository
    {
        Task<List<PhoneNumberRange>> GetPhoneNumberRangesByPrefixAsync(string prefix);
        Task<IDbContextTransaction> BeginTransactionAsync();
    }
}
