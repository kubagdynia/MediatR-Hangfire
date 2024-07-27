using MediatRTest.Core.Responses.Domain;
using MediatRTest.Invoices.Models;

namespace MediatRTest.Invoices.Queries;

// This response is used to return the result of the GetInvoiceQuery
public sealed class GetInvoiceQueryResponse : BaseDomainResponse
{
    public Invoice? Invoice { get; set; }
}