namespace Payments.Services.Exceptions
{
    public class PaymentTransactionNotFoundException : Exception
    {
        public PaymentTransactionNotFoundException()
            : base("Транзакция не найдена.")
        { }
    }
}
