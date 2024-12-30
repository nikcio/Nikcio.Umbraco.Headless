using HotChocolate;

namespace Nikcio.UHeadless.IntegrationTests.TestProject;

internal class GraphQLErrorFilter : IErrorFilter
{
    private readonly ILogger<GraphQLErrorFilter> _logger;

    public GraphQLErrorFilter(ILogger<GraphQLErrorFilter> logger)
    {
        _logger = logger;
    }

    public IError OnError(IError error)
    {
        ArgumentNullException.ThrowIfNull(error);

        _logger.LogError(error.Exception, "Request failed");
        return error.Exception != null ? error.WithMessage(error.Exception.Message) : error;
    }
}
