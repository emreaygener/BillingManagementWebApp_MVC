using BillingManagementWebApp.Data;
using BillingManagementWebApp.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
