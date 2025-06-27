using HotChocolate;
using HotChocolate.Data;
using HotChocolate.Data.Filters;
using HotChocolate.Data.Sorting;
using HotChocolate.Types;
using Microsoft.EntityFrameworkCore;
using PaymentGateway.Client.Models;
using PaymentGateway.Server.Data;
using PaymentGateway.Server.Models;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway.Server.GraphQL
{
    public class Query
    {
        [UsePaging]
        [HotChocolate.Data.UseFiltering(typeof(PaymentResultFilterType))]
        [HotChocolate.Data.UseSorting(typeof(PaymentResultSortType))]
        public IQueryable<PaymentResult> GetPayments(
            [Service] AppDbContext context,
            int limit = 10,
            int offset = 0)
        {
            return context.PaymentResults
                .Include(p => p.PaymentRequest)
                .OrderByDescending(p => p.ProcessedAt)
                .Skip(offset)
                .Take(limit);
        }

        public async Task<PaymentResult?> GetPayment(
            [ID] string id,
            [Service] AppDbContext context)
        {
            return await context.PaymentResults
                .Include(p => p.PaymentRequest)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public string HealthCheck() => "Server is running";
    }

    public class PaymentResultFilterType : FilterInputType<PaymentResult>
    {
        protected override void Configure(IFilterInputTypeDescriptor<PaymentResult> descriptor)
        {
            descriptor.BindFieldsExplicitly();
            descriptor.Field(f => f.Status);
            descriptor.Field(f => f.ProcessedAt);
        }
    }

    public class PaymentResultSortType : SortInputType<PaymentResult>
    {
        protected override void Configure(ISortInputTypeDescriptor<PaymentResult> descriptor)
        {
            descriptor.BindFieldsExplicitly();
            descriptor.Field(f => f.ProcessedAt);
        }
    }
}