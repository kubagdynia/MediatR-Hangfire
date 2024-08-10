using MediatRTest.Api.Invoices.Contracts.V1;
using MediatRTest.Api.Invoices.Extensions;
using MediatRTest.Core.Endpoints;
using MediatRTest.Core.Messages;
using MediatRTest.Invoices.Queries;
using Microsoft.AspNetCore.Http.HttpResults;

namespace MediatRTest.Api.Invoices;

public static class GetInvoices
{
    public sealed class Endpoint : IEndpoint
    {
        // GET /invoices
        public void MapEndpoint(IEndpointRouteBuilder endpointRouteBuilder)
        {
            endpointRouteBuilder.MapGet("invoices", async (IMessageManager messageManager) =>
                {
                    // Get all invoices
                    GetInvoicesQueryResponse result = await messageManager.SendCommand(new GetInvoicesQuery());

                    // If there are no invoices, return 204 No Content
                    if (result.Invoices is null || !result.Invoices.Any())
                    {
                        return Results.NoContent();
                    }

                    // Return the list of invoices
                    return Results.Ok(result.Invoices
                        .Select(i => i.ToInvoiceResponse()).ToList());
                })
                .WithTags(nameof(Invoices))
                .WithName(nameof(GetInvoices))
                .WithSummary("Returns a list of all invoices")
                .Produces<List<InvoiceResponse>>()
                .Produces<NoContent>(StatusCodes.Status204NoContent);
        }
    }
}