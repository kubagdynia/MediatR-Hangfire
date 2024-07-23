using MediatRTest.Api.Endpoints;
using MediatRTest.Core.Messages;
using MediatRTest.Invoices.Commands;

namespace MediatRTest.Api.Invoices;

public static class CreateInvoice
{
    public record Request(string Number, DateTime CreationDate);

    public record Response(string Id, string Number, DateTime CreationDate);
    
    public sealed class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder endpointRouteBuilder)
        {
            endpointRouteBuilder.MapPost("invoices",  async (Request request, IMessageManager messageManager) =>
                {
                    CreateInvoiceCommandResponse result =
                        await messageManager.SendCommand(new CreateInvoiceCommand(request.Number,
                            request.CreationDate));
                    
                    return Results.Ok(new Response(result.Id, result.Number, result.CreationDate));
                })
                .WithTags("Invoices")
                .WithSummary("Creates a new invoice")
                .Produces<Response>()
                .Produces(StatusCodes.Status400BadRequest);
        }
    }
}