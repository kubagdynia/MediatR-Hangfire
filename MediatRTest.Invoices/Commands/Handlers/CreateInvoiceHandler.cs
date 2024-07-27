using MediatR;
using MediatRTest.Invoices.Models;
using MediatRTest.Invoices.Repositories;

namespace MediatRTest.Invoices.Commands.Handlers;

// This handler is used to create a new invoice
internal sealed class CreateInvoiceHandler(IInvoiceRepository repository) : IRequestHandler<CreateInvoiceCommand, CreateInvoiceCommandResponse>
{
    public Task<CreateInvoiceCommandResponse> Handle(CreateInvoiceCommand request, CancellationToken cancellationToken)
    {
        var invoiceId = Guid.NewGuid().ToString();
        
        var invoice = new DbInvoice(invoiceId, request.Number, request.Amount, request.CreationDate);
        
        repository.Create(invoice);

        var response = new CreateInvoiceCommandResponse
            { Id = invoice.Id, Number = invoice.Number, Amount = invoice.Amount, CreationDate = invoice.CreationDate };

        return Task.FromResult(response);
    }
}