namespace Transaction.Services.Exceptions
{
    public class InsufficientFundsException : Exception
    {
        public InsufficientFundsException()
            : base("Недостаточно средств для выполнения операции!")
        { }
    }
}
