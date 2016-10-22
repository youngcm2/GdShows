using System;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Models.Api;

namespace GdShows.Filters
{
    public class GlobalExceptionFilter : IExceptionFilter, IDisposable
    {
        private readonly string _env;

        public GlobalExceptionFilter(string env)
        {
            _env = env;
        }

        public void Dispose()
        {

        }

        public void OnException(ExceptionContext context)
        {

            var stackTrace = new[] { "No stack trace available" };

            if (_env != "Production")
            {
                stackTrace = context.Exception.StackTrace.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            }

            var apiException = context.Exception as ApiException;
            var statusCode = 500;
            object content = null;
            var message = context.Exception.Message;
            if (apiException != null)
            {
                statusCode = (int)apiException.StatusCode;
                content = apiException.Content;
                if (!string.IsNullOrEmpty(apiException.Message))
                {
                    message = apiException.Message;
                }
            }

            var dbEntityValidationException = context.Exception as DbEntityValidationException;
            if (dbEntityValidationException != null)
            {
                message = HandleDbValidationException(dbEntityValidationException);
            }

            var dbUpdateException = context.Exception as DbUpdateException;
            if (dbUpdateException != null)
            {
                message = HandleDbUpdateException(dbUpdateException);
            }

            var response = new
            {
                Message = message,
                Content = content,
                StackTrace = stackTrace
            };

            context.Result = new ObjectResult(response)
            {
                StatusCode = statusCode
            };
        }

        private string HandleDbValidationException(DbEntityValidationException exc)
        {
            var message = "Error trying to save EF changes";
            // just to ease debugging
            foreach (var error in exc.EntityValidationErrors)
            {
                message += string.Join("\r\n", error.ValidationErrors.Select(e => $" - {e.ErrorMessage}"));
            }

            return message;
        }

        private string HandleDbUpdateException(DbUpdateException e)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"DbUpdateException error details - {e?.InnerException?.InnerException?.Message}");

            foreach (var eve in e.Entries)
            {
                sb.AppendLine($"Entity of type {eve.Entity.GetType().Name} in state {eve.State} could not be updated");
            }

            return sb.ToString();

        }
    }
}
