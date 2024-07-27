using MediatRTest.Core.Endpoints;
using MediatRTest.Core.Messages;
using MediatRTest.Invoices.Queries;
using Microsoft.AspNetCore.Http.HttpResults;

namespace MediatRTest.Api.Invoices;

public static class GetInvoice
{
    public record Response(string Id, string Number, decimal Amount, DateTime CreationDate);
    
    public sealed class Endpoint : IEndpoint
    {
        // GET /invoices/{id}
        public void MapEndpoint(IEndpointRouteBuilder endpointRouteBuilder)
        {
            // Get the invoice with the indicated id
            endpointRouteBuilder.MapGet("invoices/{id}", async (string id, IMessageManager messageManager) =>
            {
                // Get the invoice
                GetInvoiceQueryResponse result = await messageManager.SendCommand(new GetInvoiceQuery(id));

                // If the invoice is not found, return 404 Not Found
                return result.Invoice is null
                    ? Results.NotFound("Not found")
                    : Results.Ok(new Response(result.Invoice.Id, result.Invoice.Number, result.Invoice.Amount, result.Invoice.CreationDate));
            })
            .WithTags("Invoices")
            .WithSummary("Returns the indicated invoice")
            .Produces<Response>()
            .Produces<NoContent>(StatusCodes.Status400BadRequest)
            .Produces<NoContent>(StatusCodes.Status404NotFound);
        }
    }
}