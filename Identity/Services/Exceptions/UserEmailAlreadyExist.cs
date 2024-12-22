namespace Identity.Services.Exceptions
{
    public class UserEmailAlreadyExist : Exception
    {
        public UserEmailAlreadyExist()
            : base("Пользователь с такой почтой уже зарегистрирован!")
        { }
    }
}
