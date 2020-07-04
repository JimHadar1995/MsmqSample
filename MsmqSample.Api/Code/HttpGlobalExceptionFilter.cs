using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MsmqSample.Common.Exceptions;

namespace MsmqSample.Api.Code
{
    /// <inheritdoc />
    public class HttpGlobalExceptionFilter : IExceptionFilter
    {
        /// <inheritdoc />
        public void OnException(ExceptionContext context)
        {
            if (context == null)
                return;
            switch (context.Exception)
            {
                case BaseMsmqSampleException msmqEx:
                    context.Result = new BadRequestObjectResult(msmqEx.Message);
                    break;
                default:
                    {
                        context.Result = new BadRequestObjectResult("Unhandled exception");
                        break;
                    }
            }
        }
    }
}
