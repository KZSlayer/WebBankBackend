using Microsoft.EntityFrameworkCore.Storage;

namespace Payments.Repositories
{
    public interface IPhoneNumberRangesRepository
    {
        Task<int?> GetPaymentProviderIdAsync(string prefix, long number);
        Task<IDbContextTransaction> BeginTransactionAsync();
    }
}
