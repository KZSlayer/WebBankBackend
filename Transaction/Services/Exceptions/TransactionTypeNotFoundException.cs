namespace Transaction.Services.Exceptions
{
    public class TransactionTypeNotFoundException : Exception
    {
        public TransactionTypeNotFoundException() : base("Такого типа транзакции не существует!") { }
    }
}
