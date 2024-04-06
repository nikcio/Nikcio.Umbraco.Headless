using HotChocolate.Execution.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nikcio.UHeadless.Members.TypeModules;

namespace Nikcio.UHeadless.Members.Extensions;

/// <summary>
/// Member extensions
/// </summary>
public static class MemberExtensions
{
    /// <summary>
    /// Adds all the Member services
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddMemberServices(this IServiceCollection services)
    {
        services
            .AddMemberRepositories()
            .AddFactories();

        return services;
    }

    /// <summary>
    /// This is for internal use only
    /// </summary>
    internal static bool UsingMemberQueries { get; set; }

    /// <summary>
    /// Adds the necessary extensions to properly use member queries.
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    public static IRequestExecutorBuilder UseMemberQueries(this IRequestExecutorBuilder builder)
    {
        UsingMemberQueries = true;

        builder.AddTypeModule<MemberTypeModule>();

        return builder;
    }
}
