namespace Identity.Services.Exceptions
{
    public class UserCreateFailedException : Exception
    {
        public UserCreateFailedException(string message) : base(message) { }
    }
}
