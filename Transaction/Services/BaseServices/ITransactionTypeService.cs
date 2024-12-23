using Transaction.DTOs;

namespace Transaction.Services.BaseServices
{
    public interface ITransactionTypeService
    {
        Task CreateTransactionTypeAsync(CreateTransactionTypeDTO transactionTypeDTO);
        Task UpdateTransactionTypeAsync(ChangeTransactionTypeDTO changeTransactionTypeDTO);
    }
}
