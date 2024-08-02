using MediatR;
using MediatRTest.Invoices.Mappers;
using MediatRTest.Invoices.Repositories;

namespace MediatRTest.Invoices.Commands.Handlers;

// This handler is used to create a new invoice
internal sealed class CreateInvoiceHandler(IInvoiceRepository repository) : IRequestHandler<CreateInvoiceCommand, CreateInvoiceCommandResponse>
{
    public Task<CreateInvoiceCommandResponse> Handle(CreateInvoiceCommand request, CancellationToken cancellationToken)
    {
        var invoiceId = Guid.NewGuid().ToString();

        var dbInvoice = request.ToDbInvoice(invoiceId);
        
        repository.Create(dbInvoice);

        var response = new CreateInvoiceCommandResponse{Invoice = dbInvoice.ToInvoice()};

        return Task.FromResult(response);
    }
}