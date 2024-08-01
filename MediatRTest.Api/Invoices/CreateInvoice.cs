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
                    CreateInvoiceCommandResponse result = await messageManager.SendCommand(request.ToCreateInvoiceCommand());

                    // If the invoice is not created, return 400 Bad Request
                    // Otherwise, return 201 Created
                    // The location header is set to the URI of the new invoice
                    // The response body is the new invoice
                    var locationUri =
                        linkGenerator.GetUriByName(httpContext, "GetInvoice", new { id = result.Invoice!.Id });
                    
                    return Results.Created(locationUri, result.Invoice?.ToInvoiceResponse());
                })
                .WithTags("Invoices")
                .WithSummary("Creates a new invoice")
                .Produces<InvoiceResponse>(StatusCodes.Status201Created)
                .Produces(StatusCodes.Status400BadRequest);
        }
    }
}