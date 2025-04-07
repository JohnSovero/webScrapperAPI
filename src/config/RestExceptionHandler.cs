using backend.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace backend.Config
{
    public class RestExceptionHandler : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            if (context.Exception is ValidationException)
            {
                var response = new
                {
                    status = (int)HttpStatusCode.BadRequest,
                    title = "Errores de validación",
                    detail = "Los datos enviados no son válidos.",
                    errors = context.ModelState.ToDictionary(
                        kvp => kvp.Key,
                        kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                    )
                };

                context.Result = new JsonResult(response)
                {
                    StatusCode = (int)HttpStatusCode.BadRequest
                };
                context.ExceptionHandled = true;
            }
            else
            {
                var exception = context.Exception;
                var statusCode = HttpStatusCode.InternalServerError;
                var title = "Ocurrió un error inesperado.";
                var detail = exception.Message;

                if (exception is ResourceNotFoundException)
                {
                    statusCode = HttpStatusCode.NotFound;
                    title = "Recurso no encontrado.";
                }
                else if (exception is ResourceDuplicateException)
                {
                    statusCode = HttpStatusCode.Conflict;
                    title = "Conflicto de recurso.";
                }
                else if (exception is BadRequestException)
                {
                    statusCode = HttpStatusCode.BadRequest;
                    title = "Solicitud incorrecta.";
                }

                var response = new
                {
                    status = (int)statusCode,
                    title,
                    detail,
                    instance = context.HttpContext.Request.Path
                };

                context.Result = new JsonResult(response)
                {
                    StatusCode = (int)statusCode
                };

                context.ExceptionHandled = true;
            }
        }
    }
}
