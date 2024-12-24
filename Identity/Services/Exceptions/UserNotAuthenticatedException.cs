namespace Identity.Services.Exceptions
{
    public class UserNotAuthenticatedException : Exception
    {
        public UserNotAuthenticatedException()
            : base("Пользователь не аутентифицирован.")
        { }
    }
}
