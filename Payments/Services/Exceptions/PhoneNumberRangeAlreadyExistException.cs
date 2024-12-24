namespace Payments.Services.Exceptions
{
    public class PhoneNumberRangeAlreadyExistException : Exception
    {
        public PhoneNumberRangeAlreadyExistException()
            : base("Диапазон телефонных номеров с указанными параметрами уже существует.")
        { }
    }
}
