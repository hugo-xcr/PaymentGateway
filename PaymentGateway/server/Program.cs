using HotChocolate.AspNetCore;
using HotChocolate.Types.Pagination;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PaymentGateway.Server.Data;
using PaymentGateway.Server.GraphQL;
using PaymentGateway.Server.Models;
using System.IO;

var builder = WebApplication.CreateBuilder(args);

var dbPath = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "payments.db");
Console.WriteLine($"Database path: {dbPath}");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite($"Data Source={dbPath};Foreign Keys=True"));

builder.Services
    .AddGraphQLServer()
    .AddQueryType<Query>()
    .AddMutationType<Mutation>()
    .AddSubscriptionType<Subscription>()
    .AddType<PaymentResultType>()
    .AddType<PaymentRequestType>()
    .AddFiltering()
    .AddSorting()
    .SetPagingOptions(new PagingOptions
    {
        MaxPageSize = 50,
        DefaultPageSize = 10,
        IncludeTotalCount = true
    })
    .ModifyRequestOptions(opt =>
    {
        opt.IncludeExceptionDetails = true;
    })
    .AddInMemorySubscriptions();

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await db.Database.EnsureCreatedAsync();

    if (!db.PaymentResults.Any())
    {
        var testRequest = new PaymentRequest
        {
            Amount = 100m,
            Currency = "USD",
            Method = "CreditCard",
            Description = "Test payment"
        };

        db.PaymentResults.Add(new PaymentResult
        {
            Status = PaymentStatus.CONFIRMED,
            TransactionId = "txn_" + Guid.NewGuid().ToString("N")[..8],
            PaymentRequest = testRequest
        });
        await db.SaveChangesAsync();
    }
}

app.UseWebSockets();
app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.MapGraphQL();
    endpoints.MapGet("/health", () => Results.Json(new
    {
        status = "Healthy",
        dbPath,
        timestamp = DateTime.UtcNow
    }));
});

app.Run();