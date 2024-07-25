using MediatRTest.Core.Responses.Domain;
using MediatRTest.Invoices.Models;

namespace MediatRTest.Invoices.Queries;

public sealed class GetInvoicesQueryResponse : BaseDomainResponse
{
    public IEnumerable<Invoice>? Invoices { get; set; }
}