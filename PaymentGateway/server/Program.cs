using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PaymentGateway.Server.Data;
using PaymentGateway.Server.GraphQL;
using PaymentGateway.Server.Models;
using System.IO;

var builder = WebApplication.CreateBuilder(args);

// Конфигурация базы данных
var dbPath = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "payments.db");
Console.WriteLine($"Database path: {dbPath}");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite($"Data Source={dbPath};Foreign Keys=True"));

// Добавляем GraphQL
builder.Services
    .AddGraphQLServer()
    .AddQueryType<Query>()
    .AddMutationType<MutationType>() // Добавьте этот тип
    .AddType<PaymentResultType>()    // Добавьте типы
    .AddType<PaymentRequestType>()
    .AddFiltering()
    .AddSorting();

var app = builder.Build();

// Инициализация базы данных
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    try
    {
        if (File.Exists(dbPath)) File.Delete(dbPath);
        await db.Database.EnsureCreatedAsync();


        // В Program.cs сервера:
        var testRequest = new PaymentRequest // Это PaymentGateway.Server.Models.PaymentRequest
        {
            Id = Guid.NewGuid().ToString(),
            Amount = 100m,
            Currency = "USD",
            Method = "TEST",
            Description = "Test record",
            CreatedAt = DateTime.UtcNow
        };

        db.PaymentRequests.Add(testRequest);
        await db.SaveChangesAsync();

        Console.WriteLine("Test data inserted successfully");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Database initialization failed: {ex}");
        throw;
    }
}

// Настройка маршрутов
app.UseRouting();
app.UseEndpoints(endpoints =>
{
    endpoints.MapGraphQL();
    endpoints.MapGet("/", () => "Payment Gateway API");
    endpoints.MapGet("/health", () => Results.Ok(new { status = "Healthy", dbPath }));
});

app.Run();