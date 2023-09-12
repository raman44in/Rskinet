using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using API.Errors;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace API.Middleware
{
    public class ExceptionMiddleware 
    {
        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        public RequestDelegate _next { get; }
        public ILogger<ExceptionMiddleware> _logger { get; }
        public IHostEnvironment _env { get; }
    

    public async Task InvokeAsync(HttpContext httpContext){
        try
        {
                await _next(httpContext);
                }
        catch(Exception ex)
        {
            _logger.LogError(ex,ex.Message);
            httpContext.Response.ContentType="application/json";
            httpContext.Response.StatusCode=(int)HttpStatusCode.InternalServerError;
            
            var res=_env.IsDevelopment() 
            ? new ApiException((int)HttpStatusCode.InternalServerError,ex.Message,ex.StackTrace.ToString()) 
            : new ApiException((int)HttpStatusCode.InternalServerError);

            var options =new JsonSerializerOptions{
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };

            var json=JsonSerializer.Serialize(res, options);
            await httpContext.Response.WriteAsync(json);

        }

    }
}
}