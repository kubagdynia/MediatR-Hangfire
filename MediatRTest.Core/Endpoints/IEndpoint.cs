namespace MediatRTest.Core.Endpoints;

// Interface is used to define the endpoint
public interface IEndpoint
{
    // Method is used to map the endpoint to the route builder
    void MapEndpoint(IEndpointRouteBuilder endpointRouteBuilder);
}