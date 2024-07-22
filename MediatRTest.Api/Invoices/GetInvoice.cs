using MediatRTest.Api.Endpoints;
using MediatRTest.Core.Messages;
using MediatRTest.Invoices.Queries;
using Microsoft.AspNetCore.Http.HttpResults;

namespace MediatRTest.Api.Invoices;

public static class GetInvoice
{
    public record Response(string Id, string Number, DateTime CreationDate);
    
    public sealed class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder endpointRouteBuilder)
        {
            endpointRouteBuilder.MapGet("invoices/{id}", async (string id, IMessageManager messageManager) =>
            {
                GetInvoiceQueryResponse result = await messageManager.SendCommand(new GetInvoiceQuery(id));

                return result.Invoice is null
                    ? Results.NotFound("Not found")
                    : Results.Ok(new Response(result.Invoice.Id, result.Invoice.Number, result.Invoice.CreationDate));
            })
            .WithTags("Invoices")
            .WithSummary("Returns the indicated invoice")
            .Produces<Response>()
            .Produces<NoContent>(StatusCodes.Status400BadRequest)
            .Produces<NoContent>(StatusCodes.Status404NotFound);
        }
    }
}