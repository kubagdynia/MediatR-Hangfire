using MediatRTest.Api.Endpoints;
using MediatRTest.Core.Messages;
using MediatRTest.Invoices.Commands;
using Microsoft.AspNetCore.Http.HttpResults;

namespace MediatRTest.Api.Invoices;

public static class DeleteInvoice
{
    public sealed class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder endpointRouteBuilder)
        {
            endpointRouteBuilder.MapDelete("invoices", async (string id, IMessageManager messageManager) =>
            {
                RemoveInvoiceCommandResponse result = await messageManager.SendCommand(new RemoveInvoiceCommand(id));
                
                return result.Removed ? Results.NoContent() : Results.NotFound("Not found");
            })
            .WithTags("Invoices")
            .WithSummary("Deletes the indicated invoice")
            .Produces<NoContent>(StatusCodes.Status204NoContent)
            .Produces<NotFound>(StatusCodes.Status404NotFound);
        }
    }
}