using HotChocolate.Types;
using PaymentGateway.Server.Models;

namespace PaymentGateway.Server.GraphQL
{
    public class PaymentRequestType : ObjectType<PaymentRequest>
    {
        protected override void Configure(IObjectTypeDescriptor<PaymentRequest> descriptor)
        {
            descriptor.Field(f => f.Id).Type<IdType>();
            descriptor.Field(f => f.Amount).Type<FloatType>();
            descriptor.Field(f => f.Currency).Type<StringType>();
            descriptor.Field(f => f.Method).Type<StringType>();
            descriptor.Field(f => f.Description).Type<StringType>();
            descriptor.Field(f => f.CreatedAt).Type<DateTimeType>();
        }
    }
}