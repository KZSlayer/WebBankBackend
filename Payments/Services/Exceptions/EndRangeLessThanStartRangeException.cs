namespace Payments.Services.Exceptions
{
    public class EndRangeLessThanStartRangeException : Exception
    {
        public EndRangeLessThanStartRangeException()
            : base("Конечный диапазон не может быть меньше начального диапазона существующего диапазона телефонных номеров.")
        { }
    }
}
