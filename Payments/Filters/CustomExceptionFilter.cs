using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Payments.Services.Exceptions;

namespace Payments.Filters
{
    public class CustomExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<CustomExceptionFilter> _logger;
        public CustomExceptionFilter(ILogger<CustomExceptionFilter> logger)
        {
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            _logger.LogError(context.Exception, "Произошла ошибка: {Message}", context.Exception.Message);
            (int statusCode, string message) = context.Exception switch
            {
                ArgumentException => (400, "Некорректный запрос. Проверьте переданные данные."),
                InvalidOperationException => (409, "Конфликт. Запрос невозможно обработать."),
                KeyNotFoundException => (404, "Ресурс не найден."),
                UnauthorizedAccessException => (401, "Доступ запрещён."),
                ProviderNotFoundException => (404, "Провайдер для номера не найден."),
                СategoryNotFoundException => (404, "Категория не найдена."),
                PaymentTransactionNotFoundException => (404, "Транзакция не найдена."),
                PaymentTransactionUpdateException => (404, "Транзакция не найдена."),
                PaymentTransactionAddException => (500, "Ошибка добавления транзакции."),
                UserNotAuthenticatedException => (401, "Ошибка! Пользователь не авторизирован."),
                _ => (500, "Произошла внутренняя ошибка сервера.")
            };
            var response = new
            {
                Error = message,
                Details = context.Exception.Message
            };
            context.Result = new ObjectResult(response)
            {
                StatusCode = statusCode
            };
            context.ExceptionHandled = true;
        }
    }
}
