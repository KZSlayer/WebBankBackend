namespace Payments.Services.Exceptions
{
    public class ServiceCategoryAlreadyExistException : Exception
    {
        public ServiceCategoryAlreadyExistException()
            : base("Такая категория уже существует!")
        { }
    }
}
