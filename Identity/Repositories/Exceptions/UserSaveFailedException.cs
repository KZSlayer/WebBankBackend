namespace Identity.Repositories.Exceptions
{
    public class UserSaveFailedException : Exception
    {
        public UserSaveFailedException(string message) : base(message) { }
    }
}
