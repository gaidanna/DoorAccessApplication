using DoorAccessApplication.Api;
using DoorAccessApplication.Api.Filters;
using DoorAccessApplication.Core;
using DoorAccessApplication.Core.Interfaces;
using DoorAccessApplication.Infrastructure;
using DoorAccessApplication.Infrastructure.Persistence;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.IdentityModel.Tokens.Jwt;

var builder = WebApplication.CreateBuilder(args);

AddServicesToContainer(builder);




var app =     builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger()
        .UseSwaggerUI(setup =>
    {
        setup.SwaggerEndpoint("https://localhost:7224/swagger/v1/swagger.json", "LockAccess.API V1");
        setup.OAuthClientId("swaggerui");
        setup.OAuthAppName("Swagger UI");
    });
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

using (var scope = app.Services.CreateScope())
{
    var dataContext = scope.ServiceProvider.GetRequiredService<LockDbContext>();
    dataContext.Database.Migrate();
}


app.MapControllers();

app.Run();

void AddServicesToContainer(WebApplicationBuilder builder)
{

    builder.Services.AddScoped<ILockRepository, LockRepository>();

    builder.Services.AddDbContext<LockDbContext>(options =>
        options.UseSqlServer(builder.Configuration["DbConnectionString"]));

    builder.Services.AddControllers(options =>
    {
        options.Filters.Add<UnhandledExceptionFilterAttribute>();
    })
        .AddFluentValidation(s =>
        {
            s.RegisterValidatorsFromAssemblyContaining<ApiAssemblyMarker>();
            s.DisableDataAnnotationsValidation = true;
        });
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(options =>
    {
        options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
        {
            Type = SecuritySchemeType.OAuth2,
            Flows = new OpenApiOAuthFlows()
            {
                Implicit = new OpenApiOAuthFlow()
                {
                    AuthorizationUrl = new Uri("https://localhost:7242/connect/authorize"),
                    TokenUrl = new Uri("https://localhost:7242/connect/token"),
                    Scopes = new Dictionary<string, string>()
                            {
                                { "lockAccess", "Lock access API" }
                            }
                }
            }
        });

        options.OperationFilter<AuthorizeCheckOperationFilter>();
    });

    ConfigureAuthService(builder.Services);

    ConfigureBusService(builder.Services);

    builder.Services.AddAutoMapper(typeof(ApiAssemblyMarker), typeof(ICoreAssemblyMarker));
}


void ConfigureAuthService(IServiceCollection services)
{
    // prevent from mapping "sub" claim to nameidentifier.
    JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Remove("sub");

    var identityUrl = "https://localhost:7130";//Configuration.GetValue<string>("IdentityUrl");

    services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

    }).AddJwtBearer(options =>
    {
        options.Authority = identityUrl;
        options.RequireHttpsMetadata = false;
        options.Audience = "lockAccess";
    });
}

void ConfigureBusService(IServiceCollection services)
{
   
}

