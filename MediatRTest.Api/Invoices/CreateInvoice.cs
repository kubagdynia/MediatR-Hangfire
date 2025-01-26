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
            endpointRouteBuilder.MapPost("invoices",  async (CreateInvoiceRequest request, IMessageManager messageManager, LinkGenerator linkGenerator, HttpContext httpContext) =>
                {
                    // Create a new invoice
                    CreateInvoiceCommandResponse result = await messageManager.SendCommandAsync(request.ToCreateInvoiceCommand());

                    // If the invoice is not created, return 400 Bad Request
                    // Otherwise, return 201 Created
                    // The location header is set to the URI of the new invoice
                    // The response body is the new invoice
                    string? locationUri =
                        linkGenerator.GetUriByName(httpContext, nameof(GetInvoice), new { id = result.Invoice!.Id });
                    
                    return Results.Created(locationUri, result.Invoice?.ToInvoiceResponse());
                })
                .WithTags(nameof(Invoices))
                .WithName(nameof(CreateInvoice))
                .WithSummary("Creates a new invoice")
                .Produces<InvoiceResponse>(StatusCodes.Status201Created)
                .Produces(StatusCodes.Status400BadRequest);
        }
    }
}