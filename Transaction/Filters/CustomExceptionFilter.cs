using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Transaction.Services.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Transaction.Filters
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
                InvalidAmountException => (400, "Ошибка! Сумма не может быть нуль или меньше нуля."),
                RecipientNotFoundException => (404, "Ошибка! Получатель с таким номером телефона не найден."),
                UserNotAuthenticatedException => (401, "Ошибка! Пользователь не авторизирован."),
                InvalidDataException => (400, "Ошибка! Переданы некорректные данные. Проверьте правильность введенных значений."),
                DbUpdateException => (409, "Ошибка! Операция конфликтует с текущими данными в базе."),
                TimeoutException => (504, "Ошибка! Время ожидания ответа от сервиса истекло. Попробуйте позже."),
                TransactionTypeAlreadyExistException => (409, "Ошибка! Такой тип транзакции уже существует в базе данных!"),
                TransactionTypeNotFoundException => (404, "Ошибка! Такого типа транзакции не существует в базе данных!"),
                InsufficientFundsException => (422, "Ошибка! На счёте недостаточно средств для выполнения операции!"),
                AccountNotFoundException => (404, "Ошибка! Такой аккаунт не найден!"),
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
