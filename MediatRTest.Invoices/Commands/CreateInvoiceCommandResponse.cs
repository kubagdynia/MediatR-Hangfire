using MediatRTest.Core.Responses.Domain;
using MediatRTest.Invoices.Models;

namespace MediatRTest.Invoices.Commands;

// This response is used to return the result of the CreateInvoiceCommand
public sealed class CreateInvoiceCommandResponse : BaseDomainResponse
{
    // NOTE!!!, do not use a constructor other than the default one as this causes a validation problem
    public Invoice? Invoice { get; set; }
}