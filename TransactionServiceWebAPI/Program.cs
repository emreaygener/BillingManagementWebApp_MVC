using Microsoft.Extensions.Options;
using MongoDB.Driver;
using TransactionServiceWebAPI.Data;
using TransactionServiceWebAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<TransactionWebApiDbSettings>(
       builder.Configuration.GetSection(nameof(TransactionWebApiDbSettings)));

builder.Services.AddSingleton<ITransactionWebApiDbSettings>(sp =>
       sp.GetRequiredService<IOptions<TransactionWebApiDbSettings>>().Value);

builder.Services.AddSingleton<IMongoClient>(s=>new MongoClient(builder.Configuration.GetValue<string>("TransactionWebApiDbSettings:ConnectionString")));

builder.Services.AddSingleton<ICardService, CardService>();


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.WebHost.UseUrls("https://localhost:5011");

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
