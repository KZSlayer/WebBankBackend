namespace Identity.Services.Exceptions
{
    public class InvalidRefreshTokenException : Exception
    {
        public InvalidRefreshTokenException()
            : base("Неверный или истёкший токен.")
        {
        }
    }
}
