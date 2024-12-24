namespace Payments.Services.Exceptions
{
    public class PhoneNumberRangeNotFoundException : Exception
    {
        public PhoneNumberRangeNotFoundException()
            : base("Указанный диапазон телефонных номеров не найден.")
        { }
    }
}
