using Identity.Repositories.Exceptions;
using Identity.Services.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Identity.Filters
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
                UserSaveFailedException => (500, "Ошибка сохранения пользователя. Попробуйте позже."),
                UserPhoneAlreadyExist => (409, "Ошибка! Пользователь с таким номером телефона уже существует."),
                UserEmailAlreadyExist => (409, "Ошибка! Пользователь с такой почтой уже существует."),
                InvalidPasswordException => (401, "Ошибка! Указан неверный пароль."),
                UserNotFoundException => (404, "Ошибка! Пользователь с указанным номером телефона не найден."),
                InvalidRefreshTokenException => (401, "Неверный или истёкший токен."),
                UserNotAuthenticatedException => (401, "Ошибка! Пользователь не аутентифицирован."),
                UserCreateFailedException => (500, "Ошибка! Не удалось сохранить пользователя в базе данных."),
                UnderageRegistrationException => (400, "Ошибка! Вам должно быть минимум 18 лет для регистрации."),
                FutureDateOfBirthException => (400, "Ошибка! Дата рождения не может быть в будущем."),
                TokenNotFoundException => (404, "Ошибка! Refresh token не найден в базе."),
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
