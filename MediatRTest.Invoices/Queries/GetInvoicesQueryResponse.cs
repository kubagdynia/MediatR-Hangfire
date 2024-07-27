using MediatRTest.Core.Responses.Domain;
using MediatRTest.Invoices.Models;

namespace MediatRTest.Invoices.Queries;

// This response is used to return the result of the GetInvoicesQuery
public sealed class GetInvoicesQueryResponse : BaseDomainResponse
{
    public IEnumerable<Invoice>? Invoices { get; set; }
}