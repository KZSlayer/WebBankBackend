namespace Payments.Services.Exceptions
{
    public class ProviderNotFoundException : Exception
    {
        public ProviderNotFoundException()
            : base("Провайдер для номера не найден.")
        { }
    }
}
