using System;
using System.Net;

namespace Models.Api
{
    public class ApiException : Exception
    {
        public HttpStatusCode StatusCode { get; set; }
        public object Content { get; set; }

        public ApiException(HttpStatusCode statusCode, string message, object content = null): this(statusCode, message, null, content)
        {
        }

        public ApiException(HttpStatusCode statusCode, string message, Exception innerException, object content = null) : base(message, innerException)
        {
            StatusCode = statusCode;
            Content = content;
        }
    }
}
