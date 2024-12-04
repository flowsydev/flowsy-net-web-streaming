using Flowsy.Web.Streaming.Buffering;
using Flowsy.Web.Streaming.Chunks;
using Flowsy.Web.Streaming.Multipart;
using Microsoft.Extensions.DependencyInjection;

namespace Flowsy.Web.Streaming;

/// <summary>
/// Represents a builder for configuring streaming services.
/// </summary>
public class StreamingBuilder
{
    private readonly IServiceCollection _services;

    internal StreamingBuilder(IServiceCollection services)
    {
        _services = services;
    }
    
    /// <summary>
    /// Registers an IBufferingProvider service.
    /// </summary>
    /// <param name="configure">
    /// An optional delegate to configure the <see cref="FileBufferingOptions"/>.
    /// </param>
    /// <returns>
    /// The current <see cref="StreamingBuilder"/>.
    /// </returns>
    public StreamingBuilder UseBuffering(Action<FileBufferingOptions, IServiceProvider>? configure = null)
    {
        var optionsBuilder = _services.AddOptions<FileBufferingOptions>();
        if (configure is not null)
            optionsBuilder.Configure(configure);
        
        _services.AddSingleton<IBufferingProvider, BufferingProvider>();
        return this;
    }
    
    /// <summary>
    /// Registers an IMultipartHandler service.
    /// </summary>
    /// <param name="configure">
    /// An optional delegate to configure the <see cref="MultipartHandlerOptions"/>.
    /// </param>
    /// <returns>
    /// The current <see cref="StreamingBuilder"/>.
    /// </returns>
    public StreamingBuilder UseMultipartRequests(Action<MultipartHandlerOptions, IServiceProvider>? configure = null)
    {
        var optionsBuilder = _services.AddOptions<MultipartHandlerOptions>();
        if (configure is not null)
            optionsBuilder.Configure(configure);
        
        _services.AddSingleton<IMultipartHandler, MultipartHandler>();
        return this;
    }
    
    /// <summary>
    /// Registers an IChunkedFileUploadHandler service.
    /// </summary>
    /// <returns></returns>
    public StreamingBuilder UseChunkedFileUploads()
    {
        _services.AddSingleton<IChunkedFileUploadHandler, ChunkedFileUploadHandler>();
        return this;
    }
}