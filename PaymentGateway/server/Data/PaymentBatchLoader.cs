using Microsoft.EntityFrameworkCore;
using PaymentGateway.Client.Models;
using PaymentGateway.Server.Data;
using PaymentGateway.Server.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PaymentGateway.server.Data;

public class PaymentBatchLoader : BatchDataLoader<string, PaymentResult>
{
    private readonly AppDbContext _dbContext;

    public PaymentBatchLoader(
        IBatchScheduler batchScheduler,
        AppDbContext dbContext,
        DataLoaderOptions? options = null)
        : base(batchScheduler, options ?? new DataLoaderOptions())
    {
        _dbContext = dbContext;
    }

    protected override async Task<IReadOnlyDictionary<string, PaymentResult>> LoadBatchAsync(
        IReadOnlyList<string> ids,
        CancellationToken cancellationToken)
    {
        return await _dbContext.PaymentResults
            .Where(p => ids.Contains(p.Id))
            .Include(p => p.PaymentRequest)
            .ToDictionaryAsync(p => p.Id, cancellationToken);
    }
}