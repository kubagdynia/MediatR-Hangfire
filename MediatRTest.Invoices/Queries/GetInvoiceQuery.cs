using MediatR;

namespace MediatRTest.Invoices.Queries;

public sealed class GetInvoiceQuery(string id) : IRequest<GetInvoiceQueryResponse>
{
    public string Id { get; } = id;
}