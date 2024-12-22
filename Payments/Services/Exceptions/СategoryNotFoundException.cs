namespace Payments.Services.Exceptions
{
    public class СategoryNotFoundException : Exception
    {
        public СategoryNotFoundException()
            : base("Провайдер для номера не найден.")
        { }
    }
}
