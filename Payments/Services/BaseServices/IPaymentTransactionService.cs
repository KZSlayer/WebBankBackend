namespace Payments.Services.BaseServices
{
    public interface IPaymentTransactionService
    {
        Task CreatePaymentTransactionAsync(int userID, int serviceCategoryId, decimal amount);
        Task UpdatePaymentTransactionStatusAsync(int transactionID, string status);
    }
}
