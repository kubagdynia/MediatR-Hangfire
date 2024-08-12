using MediatR;
using MediatRTest.Data;
using MediatRTest.Invoices.Mappers;

namespace MediatRTest.Invoices.Commands.Handlers;

// This handler is used to create a new invoice
internal sealed class CreateInvoiceHandler(DataContext dataContext) : IRequestHandler<CreateInvoiceCommand, CreateInvoiceCommandResponse>
{
    public async Task<CreateInvoiceCommandResponse> Handle(CreateInvoiceCommand request, CancellationToken cancellationToken)
    {
        // Generate a new invoice ID
        var invoiceId = Guid.NewGuid().ToString();

        // Convert the CreateInvoiceCommand to a DbInvoice
        var invoice = request.ToDbInvoice(invoiceId);
        
        // Add the invoice to the database
        dataContext.Invoices.Add(invoice);
        await dataContext.SaveChangesAsync(cancellationToken);
        
        // Return the invoice
        var response = new CreateInvoiceCommandResponse{Invoice = invoice.ToInvoice()};
        
        return response;
    }
}