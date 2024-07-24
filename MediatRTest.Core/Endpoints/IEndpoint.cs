namespace MediatRTest.Core.Endpoints;

public interface IEndpoint
{
    void MapEndpoint(IEndpointRouteBuilder endpointRouteBuilder);
}