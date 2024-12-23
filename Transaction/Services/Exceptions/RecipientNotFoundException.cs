namespace Transaction.Services.Exceptions
{
    public class RecipientNotFoundException : Exception
    {
        public RecipientNotFoundException()
            : base("Получатель не найден!")
        { }
    }
}
