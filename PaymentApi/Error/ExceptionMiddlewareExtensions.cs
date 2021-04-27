using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using PaymentCommon.Interfaces;
using PaymentCommon.Models;

namespace PaymentApi.Error
{
    /// <summary>
    /// Error middleware.
    /// </summary>
    public static class ExceptionMiddlewareExtensions
    {
        /// <summary>
        /// Configure exception handler.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="logger"></param>
        public static void ConfigureExceptionHandler(this IApplicationBuilder app, ILoggerManager logger)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    var code = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";
                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeature != null)
                    {
                        var exception = contextFeature.Error;
                        logger.LogError($"Something went wrong: {exception}");

                        context.Response.StatusCode = code;
                        var traceId = Activity.Current?.Id;
                        await context.Response.WriteAsync(new CustomErrorDetails()
                        {
                            Status = context.Response.StatusCode,
                            TraceId = traceId,
                            Message = contextFeature.Error?.Message ?? "Internal Server Error."
                        }.ToString());
                    }
                });
            });
        }
    }
}
