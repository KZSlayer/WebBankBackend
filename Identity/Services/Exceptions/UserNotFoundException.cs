namespace Identity.Services.Exceptions
{
    public class UserNotFoundException : Exception
    {
        public UserNotFoundException()
        : base("Пользователь с указанным номером телефона не найден.")
        {
        }
    }
}
