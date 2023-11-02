using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Shared.Services;
using Web.Extensions;
using Web.Handlers;
using Web.Helpers;
using Web.Models;
using Web.Services;
using Web.Services.Interfaces;
using Web.Validators;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpContextAccessor();
builder.Services.Configure<ServiceApiSettings>(builder.Configuration.GetSection("ServiceApiSettings"));
builder.Services.Configure<ClientSettings>(builder.Configuration.GetSection("ClientSettings"));
builder.Services.AddHttpClientServices(builder.Configuration);
builder.Services.AddSingleton<PhotoHelper>();
builder.Services.AddScoped<ResourceOwnerPasswordTokenHandler>();
builder.Services.AddScoped<ClientCredentialTokenHandler>();
builder.Services.AddScoped<ISharedIdentityService, SharedIdentityService>();
builder.Services.AddScoped<IClientCredentialTokenService, ClientCredentialTokenService>();
builder.Services.AddAccessTokenManagement();
// todo : fluent validation türkçe çalýþmýyor
builder.Services.AddControllersWithViews().AddFluentValidation(opt =>
{
    opt.DisableDataAnnotationsValidation = true;
    opt.RegisterValidatorsFromAssemblyContaining<CourseCreateInputValidator>();
    opt.AutomaticValidationEnabled = false;
});
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(opt =>
    {
        opt.LoginPath = "/Auth/Signin";
        opt.ExpireTimeSpan = TimeSpan.FromDays(60);
        opt.SlidingExpiration = true;
        opt.Cookie.Name = "webcookie";
    });

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.Run();

