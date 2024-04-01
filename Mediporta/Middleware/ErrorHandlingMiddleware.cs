using Mediporta.Exceptions;

namespace Mediporta.Middleware
{
    public class ErrorHandlingMiddleware : IMiddleware
    {
        private readonly ILogger<ErrorHandlingMiddleware> _logger;

        public ErrorHandlingMiddleware(ILogger<ErrorHandlingMiddleware> logger)
        {
            _logger = logger;
        }
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
            await next.Invoke(context);
            }
            catch(BadRequestException e)
            {
                _logger.LogInformation(e, e.Message);
                context.Response.StatusCode = 400;
                await context.Response.WriteAsync(e.Message);
            }
            catch (UrlException e)
            {
                _logger.LogInformation(e, e.Message);
                context.Response.StatusCode = 400;
                await context.Response.WriteAsync(e.Message);
            }
            catch (NotFoundException e)
            {
                _logger.LogInformation(e, e.Message);
                context.Response.StatusCode = 404;
                await context.Response.WriteAsync(e.Message);
            }
            catch (MissingAddressException e)
            {
                _logger.LogWarning(e, e.Message);
                context.Response.StatusCode = 500;
                await context.Response.WriteAsync(e.Message);
            }
            catch (ApiUnavailableException e)
            {
                _logger.LogInformation(e, e.Message);
                context.Response.StatusCode = 503;
                await context.Response.WriteAsync(e.Message);
            }
            catch (Exception e)
            {
                _logger.LogWarning(e, e.Message);
                context.Response.StatusCode = 500;
                await context.Response.WriteAsync("Błąd serwera");
            }
        }
    }
}
