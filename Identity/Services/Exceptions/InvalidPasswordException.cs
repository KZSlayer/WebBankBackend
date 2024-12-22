namespace Identity.Services.Exceptions
{
    public class InvalidPasswordException : Exception
    {
        public InvalidPasswordException()
        : base("Указан неверный пароль.")
        {
        }
    }
}
