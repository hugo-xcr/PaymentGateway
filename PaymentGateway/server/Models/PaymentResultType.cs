using HotChocolate.Types;
using PaymentGateway.Client.Models;
using PaymentGateway.Server.Models;

namespace PaymentGateway.Server.GraphQL
{
    public class PaymentResultType : ObjectType<PaymentResult>
    {
        protected override void Configure(IObjectTypeDescriptor<PaymentResult> descriptor)
        {
            descriptor.Field(f => f.Id).Type<IdType>();
            descriptor.Field(f => f.Status).Type<EnumType<PaymentStatus>>();
            descriptor.Field(f => f.TransactionId).Type<StringType>();
            descriptor.Field(f => f.ProcessedAt).Type<DateTimeType>();
            descriptor.Field(f => f.PaymentRequest).Type<PaymentRequestType>();
        }
    }
}