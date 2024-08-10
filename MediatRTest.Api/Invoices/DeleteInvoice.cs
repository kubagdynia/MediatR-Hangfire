using MediatRTest.Core.Endpoints;
using MediatRTest.Core.Messages;
using MediatRTest.Invoices.Commands;
using Microsoft.AspNetCore.Http.HttpResults;

namespace MediatRTest.Api.Invoices;

public static class DeleteInvoice
{
    public sealed class Endpoint : IEndpoint
    {
        // DELETE /invoices/{id}
        public void MapEndpoint(IEndpointRouteBuilder endpointRouteBuilder)
        {
            // Delete the invoice with the indicated id
            endpointRouteBuilder.MapDelete("invoices/{id}", async (string id, IMessageManager messageManager) =>
            {
                // Delete the invoice
                RemoveInvoiceCommandResponse result = await messageManager.SendCommand(new RemoveInvoiceCommand(id));
                
                // If the invoice is not found, return 404 Not Found
                return result.Removed ? Results.NoContent() : Results.NotFound("Not found");
            })
            .WithTags(nameof(Invoices))
            .WithName(nameof(DeleteInvoice))
            .WithSummary("Deletes the indicated invoice")
            .Produces<NoContent>(StatusCodes.Status204NoContent)
            .Produces<NotFound>(StatusCodes.Status404NotFound);
        }
    }
}