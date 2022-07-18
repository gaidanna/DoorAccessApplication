using DoorAccessApplication.Ids;
using DoorAccessApplication.Ids.Certificate;
using DoorAccessApplication.Ids.Interfaces;
using DoorAccessApplication.Ids.Listeners;
using DoorAccessApplication.Ids.Models;
using DoorAccessApplication.Ids.Persistence;
using DoorAccessApplication.Ids.Services;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Plain.RabbitMQ;
using RabbitMQ.Client;

var builder = WebApplication.CreateBuilder(args);

ConfigureApplication(builder);

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dataContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dataContext.Database.Migrate();
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
else
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseIdentityServer();
app.UseCookiePolicy();
app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.MapDefaultControllerRoute();
    endpoints.MapControllers();
});

app.Run();

void ConfigureApplication(WebApplicationBuilder builder)
{
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(builder.Configuration["DbConnectionString"]));

    builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                    .AddEntityFrameworkStores<ApplicationDbContext>()
                    .AddDefaultTokenProviders();

    builder.Services.AddTransient<ILoginService<ApplicationUser>, EFLoginService>();

    builder.Services.AddControllers();
    builder.Services.AddControllersWithViews();
    builder.Services.AddRazorPages();

    builder.Services.AddIdentityServer()
        .AddSigningCredential(Certificate.Get())
        .AddAspNetIdentity<ApplicationUser>()
        .AddInMemoryClients(Config.Clients)
        .AddInMemoryIdentityResources(Config.IdentityResources)
        .AddInMemoryApiResources(Config.ApiResources)
        .AddInMemoryApiScopes(Config.ApiScopes);

    builder.Services.AddTransient<IProfileService, ProfileService>();

    builder.Services.AddSingleton<IConnectionProvider>(new ConnectionProvider("amqp://guest:guest@localhost:5672"));
    builder.Services.AddSingleton<IPublisher>(x => new Publisher(x.GetService<IConnectionProvider>(),
            "identity_exchange",
            ExchangeType.Topic));
    builder.Services.AddSingleton<ISubscriber>(x => new Subscriber(x.GetService<IConnectionProvider>(),
        "user_exchange",
        "user_response",
        "user.response",
        ExchangeType.Topic));

    builder.Services.AddHostedService<DoorAccessResponseListener>();
}