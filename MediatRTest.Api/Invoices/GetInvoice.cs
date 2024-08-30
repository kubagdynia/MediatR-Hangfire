using MediatRTest.Api.Invoices.Contracts.V1;
using MediatRTest.Api.Invoices.Extensions;
using MediatRTest.Core.Endpoints;
using MediatRTest.Core.Messages;
using MediatRTest.Invoices.Queries;
using Microsoft.AspNetCore.Http.HttpResults;

namespace MediatRTest.Api.Invoices;

public static class GetInvoice
{
    public sealed class Endpoint : IEndpoint
    {
        // GET /invoices/{id}
        public void MapEndpoint(IEndpointRouteBuilder endpointRouteBuilder)
        {
            // Get the invoice with the indicated id
            endpointRouteBuilder.MapGet("invoices/{id}", async (string id, IMessageManager messageManager) =>
            {
                // Get the invoice
                GetInvoiceQueryResponse result = await messageManager.SendCommandAsync(new GetInvoiceQuery(id));

                // If the invoice is not found, return 404 Not Found
                return result.Invoice is null
                    ? Results.NotFound("Not found")
                    : Results.Ok(result.Invoice.ToInvoiceResponse());
            })
            .WithTags(nameof(Invoices))
            .WithName(nameof(GetInvoice))
            .WithSummary("Returns the indicated invoice")
            .Produces<InvoiceResponse>()
            .Produces<NoContent>(StatusCodes.Status400BadRequest)
            .Produces<NoContent>(StatusCodes.Status404NotFound);
        }
    }
}