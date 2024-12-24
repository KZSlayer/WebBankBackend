using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
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
                ServiceCategoryAlreadyExistException => (409, "Ошибка! Такая категория уже существует."),
                ServiceCategoryNotFoundException => (404, "Ошибка! Такая категория отсутствует в базе."),
                PhoneNumberRangeAlreadyExistException => (409, "Ошибка! Такой диапазон номеров уже существует."),
                PhoneNumberRangeNotFoundException => (404, "Ошибка! Такой диапазон номеров отсутствует в базе."),
                StartRangeGreaterThanEndRangeException => (400, "Ошибка! Начальный диапазон не может быть больше конечного диапазона существующего диапазона телефонных номеров."),
                EndRangeLessThanStartRangeException => (400, "Ошибка! Конечный диапазон не может быть меньше начального диапазона существующего диапазона телефонных номеров."),
                PaymentProviderAlreadyExistException => (409, "Ошибка! Такой провайдер уже существует в базе."),
                PaymentProviderNotFoundException => (404, "Ошибка! Такой провайдер отсутствует в базе."),
                DbUpdateException => (409, "Ошибка! Данные в базе данных не были обновленны."),
                OperationCanceledException => (500, "Ошибка! Операция была отменена."),
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
