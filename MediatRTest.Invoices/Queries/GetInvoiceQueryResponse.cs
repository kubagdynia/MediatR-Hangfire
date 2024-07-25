using MediatRTest.Core.Responses.Domain;
using MediatRTest.Invoices.Models;

namespace MediatRTest.Invoices.Queries;

public sealed class GetInvoiceQueryResponse : BaseDomainResponse
{
    public Invoice? Invoice { get; set; }
}