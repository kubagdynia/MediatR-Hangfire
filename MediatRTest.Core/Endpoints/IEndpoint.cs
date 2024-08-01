namespace MediatRTest.Core.Endpoints;

public interface IEndpoint
{
    // Method is used to map the endpoint to the route builder
    void MapEndpoint(IEndpointRouteBuilder endpointRouteBuilder);
}