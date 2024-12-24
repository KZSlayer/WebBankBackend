namespace Payments.Services.Exceptions
{
    public class PaymentProviderNotFoundException : Exception
    {
        public PaymentProviderNotFoundException()
            : base("Такого провайдера не существует!")
        { }
    }
}
