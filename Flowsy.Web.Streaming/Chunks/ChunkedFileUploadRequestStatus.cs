namespace Flowsy.Web.Streaming.Chunks;

/// <summary>
/// Represents the status of a chunked file upload request.
/// </summary>
public enum ChunkedFileUploadRequestStatus
{
    /// <summary>
    ///  The upload is partially completed.
    /// </summary>
    Partial,
    
    /// <summary>
    /// The upload is completed.
    /// </summary>
    Completed
}