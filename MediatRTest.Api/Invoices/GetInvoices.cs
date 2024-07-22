using MediatRTest.Api.Endpoints;
using MediatRTest.Core.Messages;
using MediatRTest.Invoices.Queries;
using Microsoft.AspNetCore.Http.HttpResults;

namespace MediatRTest.Api.Invoices;

public static class GetInvoices
{
    public record Response(string Id, string Number, DateTime CreationDate);
    
    public sealed class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder endpointRouteBuilder)
        {
            endpointRouteBuilder.MapGet("invoices", async (IMessageManager messageManager) =>
                {
                    GetInvoicesQueryResponse result = await messageManager.SendCommand(new GetInvoicesQuery());

                    if (result.Invoices is null || !result.Invoices.Any())
                    {
                        return Results.NoContent();
                    }

                    return Results.Ok(result.Invoices
                        .Select(i => new Response(i.Id.ToString(), i.Number!, i.CreationDate)).ToList());
                })
                .WithTags("Invoices")
                .WithSummary("Returns a list of all invoices")
                .Produces<List<Response>>()
                .Produces<NoContent>(StatusCodes.Status204NoContent);
        }
    }
}