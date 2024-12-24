namespace Transaction.Services.Exceptions
{
    public class TransactionTypeAlreadyExistException : Exception
    {
        public TransactionTypeAlreadyExistException()
            : base("Такой тип транзакции уже существует!")
        { }
    }
}
