using BillingManagementWebApp.Data;
using BillingManagementWebApp.Repositories;
using Microsoft.EntityFrameworkCore;
using BillingManagementWebApp.Services;
using System.Reflection;
using BillingManagementWebApp.Middlewares;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Net;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSession(opt =>
{
    opt.IdleTimeout = TimeSpan.FromMinutes(60);
});
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opt =>
{ 
    opt.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateAudience = true,
        ValidateIssuer = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Token:Issuer"],
        ValidAudience = builder.Configuration["Token:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Token:SecurityKey"])),
        ClockSkew = TimeSpan.Zero
    };
});

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<BillingManagementDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("BillingDbConnectionString")));

builder.Services.AddScoped<IBillingManagementDbContext, BillingManagementDbContext>();

builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<FlatRepository>();
builder.Services.AddScoped<DueRepository>();
builder.Services.AddScoped<InvoiceRepository>();
builder.Services.AddScoped<MessageRepository>();
builder.Services.AddSingleton<ILoggerService, LoggerService>();

builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    DataGenerator.Initialize(services);
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();

app.Use(async (context, next) =>
{
    var token = context.Session.GetString("token");
    if (!string.IsNullOrEmpty(token))
    {
        context.Request.Headers.Add("Authorization", "Bearer "+token);
    }
    await next();
});

app.UseAuthentication();
app.UseAuthorization();

app.UseCustomExceptionMiddleware();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
