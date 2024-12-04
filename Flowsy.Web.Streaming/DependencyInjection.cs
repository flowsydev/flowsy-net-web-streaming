using Microsoft.Extensions.DependencyInjection;

namespace Flowsy.Web.Streaming;

/// <summary>
/// Extension methods for setting up streaming services in an <see cref="IServiceCollection"/>.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Adds streaming services to the specified <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">
    /// The <see cref="IServiceCollection"/> to add the services to.
    /// </param>
    /// <returns>
    /// A <see cref="StreamingBuilder"/> that can be used to further configure the streaming services.
    /// </returns>
    public static StreamingBuilder AddStreaming(this IServiceCollection services) => new (services);
}