

using System.Text;
using AIC_CRM_API.Filters;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Service.EntityAudit;

namespace AIC_CRM_API.Installers
{
    public class DataInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {

            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            services.AddAuthorization();
            //services.AddAuthentication().AddCookie(IdentityConstants.ApplicationScheme)
            //.AddBearerToken(IdentityConstants.BearerScheme);

      

            services.AddIdentity<User, IdentityRole>(options =>
            {
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.User.RequireUniqueEmail = false;
                options.Password.RequiredLength = 3;
            })
            .AddEntityFrameworkStores<MainContext>()
            .AddDefaultTokenProviders();

            services.AddDbContext<MainContext>((serviceProvider, options) =>
            {
                var connectionString = configuration.GetConnectionString("DefaultConnection");
                if (string.IsNullOrEmpty(connectionString))
                {
                    throw new InvalidOperationException("Connection string 'DefaultConnection' is null or empty.");
                }

                var interceptor = serviceProvider.GetRequiredService<AuditSaveChangesInterceptor>();
                options.UseNpgsql(connectionString)
                       .AddInterceptors(interceptor);
            });


            services.AddCors(options =>
            {
                options.AddPolicy("AllowMultipleOrigins", builder =>
                {
                    builder.AllowAnyHeader()
                           .AllowAnyMethod()
                           //.WithOrigins("http://localhost:4200", "http://172.235.58.249")
                           .WithOrigins("http://localhost:4200", "http://172.233.152.32")
                           .AllowCredentials();
                });

                //options.AddPolicy("AllowAllOrigins", builder =>
                //{
                //    builder.AllowAnyHeader()
                //           .AllowAnyMethod()
                //           .AllowAnyOrigin(); // Allow all origins
                //});
            });

            services.AddControllers(options =>
            {
                //options.Filters.Add(typeof(ExceptionFilter));
                var policy = new AuthorizationPolicyBuilder()
                                .RequireAuthenticatedUser()
                                .Build();
                options.Filters.Add(new AuthorizeFilter(policy));
            })
            .AddJsonOptions(
                options =>
                {
                    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                    options.JsonSerializerOptions.PropertyNamingPolicy = null;
                    options.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
                    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                }).ConfigureApiBehaviorOptions
            (options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    return new BadRequestObjectResult(new RequestValidationFilter(context))
                    {
                        StatusCode = 400
                    };
                };
            });
        }
    }
}
