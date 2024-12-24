namespace Identity.Repositories.Exceptions
{
    public class TokenNotFoundException : Exception
    {
        public TokenNotFoundException()
            : base("Refresh token не найден в базе.")
        { }
    }
}
