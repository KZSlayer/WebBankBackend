namespace Identity.Services.Exceptions
{
    public class UserPhoneAlreadyExist : Exception
    {
        public UserPhoneAlreadyExist()
            : base("Пользователь с таким номером телефона уже зарегистрирован!")
        { }
    }
}
