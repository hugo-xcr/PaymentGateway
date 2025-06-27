using HotChocolate.Types;

namespace PaymentGateway.Server.GraphQL
{
    public class MutationType : ObjectType<Mutation>
    {
        protected override void Configure(IObjectTypeDescriptor<Mutation> descriptor)
        {
            descriptor
                .Field(f => f.InitPayment(default!, default!))
                .Type<PaymentResultType>()
                .Name("initPayment");
        }
    }
}