using MediatRTest.Api.Invoices.Contracts.V1;
using MediatRTest.Api.Invoices.Extensions;
using MediatRTest.Core.Endpoints;
using MediatRTest.Core.Messages;
using MediatRTest.Invoices.Commands;

namespace MediatRTest.Api.Invoices;

public static class CreateInvoice
{
    public sealed class Endpoint : IEndpoint
    {
        // POST /invoices
        public void MapEndpoint(IEndpointRouteBuilder endpointRouteBuilder)
        {
            endpointRouteBuilder.MapPost("invoices",  async (CreateInvoiceRequest request, IMessageManager messageManager) =>
                {
                    // Create a new invoice
                    CreateInvoiceCommandResponse result = await messageManager.SendCommand(request.ToCreateInvoiceCommand());
                    return Results.Ok(result.Invoice?.ToInvoiceResponse());
                })
                .WithTags("Invoices")
                .WithSummary("Creates a new invoice")
                .Produces<InvoiceResponse>()
                .Produces(StatusCodes.Status400BadRequest);
        }
    }
}