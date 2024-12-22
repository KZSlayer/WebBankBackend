namespace Identity.Services.Exceptions
{
    public class UnderageRegistrationException : Exception
    {
        public UnderageRegistrationException()
            : base("Вам должно быть минимум 18 лет для регистрации.")
        { }
    }
}
