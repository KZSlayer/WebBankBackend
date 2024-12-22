namespace Identity.Services.Exceptions
{
    public class UserCreateFailedException : Exception
    {
        public UserCreateFailedException()
            : base("Не удалось сохранить пользователя в базе данных.")
        { }
    }
}
