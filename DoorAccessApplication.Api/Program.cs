using DoorAccessApplication.Api;
using DoorAccessApplication.Api.Filters;
using DoorAccessApplication.Api.Listeners;
using DoorAccessApplication.Core;
using DoorAccessApplication.Core.Interfaces;
using DoorAccessApplication.Core.Services;
using DoorAccessApplication.Infrastructure;
using DoorAccessApplication.Infrastructure.Persistence;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Plain.RabbitMQ;
using RabbitMQ.Client;
using System.IdentityModel.Tokens.Jwt;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

ConfigureApplication(builder);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseSwagger()
    .UseSwaggerUI(setup =>
    {
        setup.SwaggerEndpoint("https://localhost:7224/swagger/v1/swagger.json", "LockAccess.API V1");
        setup.OAuthClientId("swaggerui");
        setup.OAuthAppName("Swagger UI");
    });

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();


using (var scope = app.Services.CreateScope())
{
    var dataContext = scope.ServiceProvider.GetRequiredService<DoorAccessDbContext>();
    dataContext.Database.Migrate();
}

app.MapControllers();

app.Run();

void ConfigureApplication(WebApplicationBuilder builder)
{
    builder.Services.AddDbContext<DoorAccessDbContext>(options =>
        options.UseSqlServer(builder.Configuration["DbConnectionString"]));
    
    ConfigureControllers(builder.Services);
    ConfigureSwagger(builder.Services);
    //builder.Services.AddEndpointsApiExplorer();
    ConfigureAuthService(builder.Services);
    ConfigureBusService(builder.Services);
    ConfigureCustomServices(builder.Services);

    builder.Services.AddAutoMapper(typeof(ApiAssemblyMarker), typeof(ICoreAssemblyMarker));
}

void ConfigureCustomServices(IServiceCollection services)
{
    builder.Services.AddScoped<ILockService, LockService>();
    builder.Services.AddScoped<ILockRepository, LockRepository>();
    builder.Services.AddScoped<ILockHistoryRepository, LockHistoryRepository>();
    builder.Services.AddScoped<IUserRepository, UserRepository>();


}

void ConfigureControllers(IServiceCollection services)
{
    services.AddControllers(options =>
    {
        options.Filters.Add<UnhandledExceptionFilterAttribute>();
    })
    .AddFluentValidation(s =>
    {
        s.RegisterValidatorsFromAssemblyContaining<ApiAssemblyMarker>();
        s.DisableDataAnnotationsValidation = true;
    })
    .AddJsonOptions(x =>
    x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
}

void ConfigureSwagger(IServiceCollection services)
{
    services.AddSwaggerGen(options =>
    {
        options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
        {
            Type = SecuritySchemeType.OAuth2,
            Flows = new OpenApiOAuthFlows()
            {
                Implicit = new OpenApiOAuthFlow()
                {
                    AuthorizationUrl = new Uri($"{builder.Configuration["IdentityUrl"]}/connect/authorize"),
                    TokenUrl = new Uri($"{builder.Configuration["IdentityUrl"]}/connect/token"),
                    Scopes = new Dictionary<string, string>()
                            {
                                { "lockAccess", "Lock access API" }
                            }
                }
            }
        });

        options.OperationFilter<AuthorizeCheckOperationFilter>();
    });
}

void ConfigureAuthService(IServiceCollection services)
{
    JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Remove("sub");

    var identityUrl = builder.Configuration["IdentityUrl"];

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
    services.AddSingleton<IConnectionProvider>(new ConnectionProvider("amqp://guest:guest@localhost:5672"));
    services.AddSingleton<IPublisher>(x => new Publisher(x.GetService<IConnectionProvider>(),
            "user_exchange",
            ExchangeType.Topic));
    services.AddSingleton<ISubscriber>(x => new Subscriber(x.GetService<IConnectionProvider>(),
        "identity_exchange",
        "identity_response",
        "identity.created",
        ExchangeType.Topic));

    services.AddHostedService<UserCreatedListener>();
}

