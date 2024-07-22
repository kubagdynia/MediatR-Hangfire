using MediatR;
using MediatRTest.Invoices.Models;
using MediatRTest.Invoices.Repositories;

namespace MediatRTest.Invoices.Commands.Handlers;

public class CreateInvoiceHandler(IInvoiceRepository repository) : IRequestHandler<CreateInvoiceCommand, CreateInvoiceCommandResponse>
{
    public Task<CreateInvoiceCommandResponse> Handle(CreateInvoiceCommand request, CancellationToken cancellationToken)
    {
        var invoice = new Invoice(request.Id, request.Number, request.CreationDate);
        
        repository.Create(invoice);

        var response = new CreateInvoiceCommandResponse(invoice.Id, invoice.Number, invoice.CreationDate);

        return Task.FromResult(response);
    }
}