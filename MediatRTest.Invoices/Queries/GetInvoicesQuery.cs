using MediatR;

namespace MediatRTest.Invoices.Queries;

public sealed class GetInvoicesQuery : IRequest<GetInvoicesQueryResponse>;