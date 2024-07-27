using MediatR;

namespace MediatRTest.Invoices.Queries;

// This query is used to get all invoices
public sealed class GetInvoicesQuery : IRequest<GetInvoicesQueryResponse>;