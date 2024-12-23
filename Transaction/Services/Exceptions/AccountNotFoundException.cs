namespace Transaction.Services.Exceptions
{
    public class AccountNotFoundException : Exception
    {
        public AccountNotFoundException()
            : base("Аккаунт не найден!")
        { }
    }
}
