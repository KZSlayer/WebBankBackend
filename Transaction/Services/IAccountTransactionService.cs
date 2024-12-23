using Transaction.DTOs;

namespace Transaction.Services
{
    public interface IAccountTransactionService
    {
        Task TopUpAccount(DepositDTO depositDTO);
        Task CashOutAccount(WithdrawDTO withdrawDTO);
        Task TransferByPhone(TransferByPhoneDTO transferDTO);
    }
}
