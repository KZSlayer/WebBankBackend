namespace Payments.Services.Exceptions
{
    public class PaymentProviderAlreadyExistException : Exception
    {
        public PaymentProviderAlreadyExistException()
            : base("Такой провайдер уже существует!")
        { }
    }
}
