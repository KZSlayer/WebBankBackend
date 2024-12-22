namespace Payments.Services.Exceptions
{
    public class PaymentTransactionAddException : Exception
    {
        public PaymentTransactionAddException(string message)
            : base(message)
        { }
    }
}
