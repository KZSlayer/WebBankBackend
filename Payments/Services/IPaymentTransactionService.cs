namespace Payments.Services
{
    public interface IPaymentTransactionService
    {
        Task CreatePaymentTransactionAsync(int userID, int serviceCategoryId, decimal amount);
    }
}
