using Microsoft.AspNetCore.Authentication.JwtBearer;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer("GatewayAuthenticationScheme", options =>
    {
        options.Authority = builder.Configuration["IdentityServerURL"];
        options.Audience = "resource_gateway";
    });

builder.Services.AddOcelot();
builder.Host.ConfigureAppConfiguration((hostingContext, config) =>
{
    config.AddJsonFile($"configuration.{hostingContext.HostingEnvironment.EnvironmentName.ToLower()}.json").AddEnvironmentVariables();
});

var app = builder.Build();

await app.UseOcelot();

app.Run();
