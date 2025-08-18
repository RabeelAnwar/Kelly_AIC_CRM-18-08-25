using Microsoft.EntityFrameworkCore.Storage;

namespace AIC_CRM_API.MiddleWares
{
    public class ExceptionHandling : IMiddleware
    {
        private readonly MainContext _dbContext;
        private readonly ILogger _logger;

        public ExceptionHandling(MainContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            IDbContextTransaction transaction = null;
            try
            {
                transaction = await _dbContext.Database.BeginTransactionAsync();
                await next(context);
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                context.Response.ContentType = "application/json";
                var result = JsonConvert.SerializeObject(new OutputDTO<dynamic>()
                {
                    Succeeded = false,
                    HttpStatusCode = Convert.ToInt32(HttpStatusCode.BadRequest),
                    Message = ex.Message,
                    Data = null
                });

                await context.Response.WriteAsync(result);

            }
        }
    }
}
