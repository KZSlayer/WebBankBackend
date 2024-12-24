namespace Payments.Services.Exceptions
{
    public class StartRangeGreaterThanEndRangeException : Exception
    {
        public StartRangeGreaterThanEndRangeException()
            : base("Начальный диапазон не может быть больше конечного диапазона существующего диапазона телефонных номеров.")
        { }
    }
}
