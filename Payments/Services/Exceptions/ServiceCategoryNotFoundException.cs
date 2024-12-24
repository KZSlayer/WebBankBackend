namespace Payments.Services.Exceptions
{
    public class ServiceCategoryNotFoundException : Exception
    {
        public ServiceCategoryNotFoundException()
            : base("Такой категории не существует!")
        { }
    }
}
