using MediatR;

namespace MediatRTest.Invoices.Queries;

public class GetInvoicesQuery : IRequest<GetInvoicesQueryResponse>;