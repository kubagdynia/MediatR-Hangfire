using MediatR;
using MediatRTest.Invoices.Models;
using MediatRTest.Invoices.Repositories;

namespace MediatRTest.Invoices.Commands.Handlers;

public class CreateInvoiceHandler(IInvoiceRepository repository) : IRequestHandler<CreateInvoiceCommand, CreateInvoiceCommandResponse>
{
    public Task<CreateInvoiceCommandResponse> Handle(CreateInvoiceCommand request, CancellationToken cancellationToken)
    {
        var invoiceId = Guid.NewGuid().ToString();
        
        var invoice = new Invoice(invoiceId, request.Number, request.CreationDate);
        
        repository.Create(invoice);

        var response = new CreateInvoiceCommandResponse
            { Id = invoice.Id, Number = invoice.Number, CreationDate = invoice.CreationDate };

        return Task.FromResult(response);
    }
}