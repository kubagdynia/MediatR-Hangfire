using MediatR;

namespace MediatRTest.Invoices.Queries;

public class GetInvoiceQuery(string id) : IRequest<GetInvoiceQueryResponse>
{
    public string Id { get; } = id;
}