namespace Identity.Services.Exceptions
{
    public class FutureDateOfBirthException : Exception
    {
        public FutureDateOfBirthException()
            : base("Дата рождения не может быть в будущем.")
        { }
    }
}
