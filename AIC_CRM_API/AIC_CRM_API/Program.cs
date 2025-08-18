
using Microsoft.Extensions.Configuration;
using Service.EntityAudit;
using Service.Services.Auth;
using Service.Services.Company;
using Service.Services.Company.DTOs;

var builder = WebApplication.CreateBuilder(args);


builder.Configuration
    .SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    .AddEnvironmentVariables();

builder.Environment.WebRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();
builder.Services.AddHttpContextAccessor();
builder.Services.InstallServiceAssembly(builder.Configuration);


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSession();
//app.UseStaticFiles();

app.UseStaticFiles(new StaticFileOptions
{
    OnPrepareResponse = ctx =>
    {
        var origin = ctx.Context.Request.Headers["Origin"].ToString();

        ctx.Context.Response.Headers.Add("Access-Control-Allow-Origin", origin);
        ctx.Context.Response.Headers.Add("Access-Control-Allow-Credentials", "true");
    }
});


app.UseCors("AllowMultipleOrigins");

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.UseMiddleware<ExceptionHandling>();

app.MapControllers();

app.UseStatusCodePages(async context =>
{
    if (
            context.HttpContext.Response.StatusCode == StatusCodes.Status401Unauthorized ||
            context.HttpContext.Response.StatusCode == StatusCodes.Status403Forbidden
        )
    {
        context.HttpContext.Response.StatusCode = Convert.ToInt32(HttpStatusCode.Unauthorized);
        context.HttpContext.Response.ContentType = "application/json";
        
    }

});




app.Run();