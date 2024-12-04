namespace Flowsy.Web.Streaming.Chunks;

/// <summary>
/// Represents a handler for chunked file uploads.
/// </summary>
public interface IChunkedFileUploadHandler
{
    /// <summary>
    /// Handles a chunked file upload request.
    /// The file chunks are stored in the <paramref name="chunkDirectoryPath"/> directory
    /// and the final reassembled file is stored in the <paramref name="targetDirectoryPath"/> directory.
    /// </summary>
    /// <param name="request">
    /// The chunked file upload request.
    /// </param>
    /// <param name="chunkDirectoryPath">
    /// The directory where the file chunks are stored.
    /// </param>
    /// <param name="targetDirectoryPath">
    /// The directory where the final reassembled file is stored.
    /// If any metadata is included in the request, it will be stored in a file with the same name as the reassembled file but with the .metadata extension.
    /// </param>
    /// <param name="cancellationToken">
    /// The cancellation token.
    /// </param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains the status of the chunked file upload request.
    /// </returns>
    Task<ChunkedFileUploadRequestStatus> HandleAsync(
        ChunkedFileUploadRequest request,
        string chunkDirectoryPath,
        string targetDirectoryPath,
        CancellationToken cancellationToken
    );
}