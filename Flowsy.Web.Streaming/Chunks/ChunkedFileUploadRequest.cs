using Microsoft.AspNetCore.Http;

namespace Flowsy.Web.Streaming.Chunks;

/// <summary>
/// Represents a chunked file upload request.
/// </summary>
public sealed class ChunkedFileUploadRequest
{
    /// <summary>
    /// Gets or sets the correlation identifier.
    /// A correlation identifier is a unique identifier that is used to correlate the chunks of a file.
    /// </summary>
    public string CorrelationId { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the original name of the file.
    /// </summary>
    public string FileName { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the chunk.
    /// </summary>
    public IFormFile Chunk { get; set; } = null!;
    
    /// <summary>
    /// Gets or sets the index of the chunk.
    /// </summary>
    public int ChunkIndex { get; set; }
    
    /// <summary>
    /// Gets or sets the total number of chunks.
    /// </summary>
    public int TotalChunks { get; set; }
    
    /// <summary>
    /// Gets or sets the metadata associated with the file.
    /// For example, the metadata can be a JSON string that contains additional information about the file.
    /// </summary>
    public string? Metadata { get; set; }
}