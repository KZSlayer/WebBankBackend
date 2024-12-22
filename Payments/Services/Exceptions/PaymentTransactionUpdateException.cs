namespace Payments.Services.Exceptions
{
    public class PaymentTransactionUpdateException : Exception
    {
        public PaymentTransactionUpdateException(string message)
            : base(message)
        { }
    }
}
