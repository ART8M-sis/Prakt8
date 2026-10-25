using FluentValidation;
using Microsoft.AspNetCore.Mvc.Filters;

namespace FileAvatar.Middleware
{
    public class ValidationFilterAttribute : ActionFilterAttribute
    {
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // проходимся по всем аргументам и проверяем их на валидность
            foreach (var argument in context.ActionArguments.Values)
            {
                if (argument is null) continue;

                var argumentType = argument.GetType();

                // ищем санитайзер для типа аргумента IValidator<T>
                var validatorType = typeof(IValidator<>).MakeGenericType(argumentType);
                var validator = context.HttpContext.RequestServices.GetService(validatorType) as IValidator;

                if (validator != null)
                {
                    // вызываем асинхронную валидацию аргумента
                    var validationContext = new ValidationContext<object>(argument);
                    var validationResult = await validator.ValidateAsync(validationContext);
                    if (!validationResult.IsValid)
                    {
                        // если валидация не прошла, прерываем ошибку 400 Bad Request в пользовательском формате
                        context.Result = new Microsoft.AspNetCore.Mvc.BadRequestObjectResult(context.ModelState);
                        return;
                    }
                }
            }
            await next();
        }
    }
}
