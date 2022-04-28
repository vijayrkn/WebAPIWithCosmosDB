using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WebAPIWithCosmosDB.Data;
using WebAPIWithCosmosDB;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<OrderContext>(options =>
    options.UseCosmos(builder.Configuration["CosmosDBConnectionString"],
    databaseName: "OrderDB"));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    await using var scope = app.Services?.GetService<IServiceScopeFactory>()?.CreateAsyncScope();
    var context = scope?.ServiceProvider.GetRequiredService<OrderContext>();
    var result = await context!.Database.EnsureCreatedAsync();

    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();

app.MapGet("/hello", () => "Hello, World!");

app.MapOrderEndpoints();

app.Run();