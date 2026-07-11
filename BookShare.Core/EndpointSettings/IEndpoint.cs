namespace BookShare.Core.EndpointSettings;

public interface IEndpoint
{
    void MapEndpoint(WebApplication app);
}