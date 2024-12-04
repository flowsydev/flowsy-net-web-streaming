namespace Flowsy.Web.Streaming.Multipart;

/// <summary>
/// Represents options for configuring multipart request handling.
/// </summary>
public class MultipartHandlerOptions
{
    /// <summary>
    /// Gets or sets the allowed MIME types for multipart requests.
    /// </summary>
    public IEnumerable<string> AllowedMimeTypes { get; set; } = [];
}