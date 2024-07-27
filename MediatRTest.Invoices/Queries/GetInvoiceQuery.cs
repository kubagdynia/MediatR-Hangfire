using MediatR;

namespace MediatRTest.Invoices.Queries;

// This query is used to get an invoice
public sealed class GetInvoiceQuery(string id) : IRequest<GetInvoiceQueryResponse>
{
    public string Id { get; } = id;
}