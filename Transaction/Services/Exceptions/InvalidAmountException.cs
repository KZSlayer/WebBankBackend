namespace Transaction.Services.Exceptions
{
    public class InvalidAmountException : Exception
    {
        public InvalidAmountException()
        : base($"Сумма не может быть нуль или меньше нуля.")
        {
        }
    }
}
