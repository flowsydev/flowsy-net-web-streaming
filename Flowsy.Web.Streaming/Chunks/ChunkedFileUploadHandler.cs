using System.Text.RegularExpressions;
using Flowsy.Web.Streaming.Resources;
using Microsoft.Extensions.Logging;

namespace Flowsy.Web.Streaming.Chunks;

/// <summary>
/// Handles chunked file upload requests.
/// </summary>
public class ChunkedFileUploadHandler : IChunkedFileUploadHandler
{
    private readonly ILogger<ChunkedFileUploadHandler>? _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="ChunkedFileUploadHandler"/> class.
    /// </summary>
    /// <param name="logger">
    /// The logger.
    /// </param>
    public ChunkedFileUploadHandler(ILogger<ChunkedFileUploadHandler>? logger = null)
    {
        _logger = logger;
    }

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
    public async Task<ChunkedFileUploadRequestStatus> HandleAsync(
        ChunkedFileUploadRequest request,
        string chunkDirectoryPath,
        string targetDirectoryPath,
        CancellationToken cancellationToken
        )
    {
        if (request.Chunk.Length == 0)
            throw new ChunkedFileUploadConflictException(Strings.FileChunkIsEmpty);

        var correlationId = request.CorrelationId;
        var temporaryMetadataFilePath = Path.Combine(chunkDirectoryPath, $"{correlationId}.metadata");
        string? metadata = null;
        if (request.ChunkIndex == 0)
        {
            if (Directory.Exists(chunkDirectoryPath))
                Directory.Delete(chunkDirectoryPath, true);
            
            Directory.CreateDirectory(chunkDirectoryPath);
            
            metadata = request.Metadata;
        }
        else if (!Directory.Exists(chunkDirectoryPath))
        {
            throw new ChunkedFileUploadConflictException(Strings.ChunkDirectoryNotFoundStartUploadFromTheBeginning);
        }
        
        if (metadata is not null)
            await File.WriteAllTextAsync(temporaryMetadataFilePath, metadata, cancellationToken);

        var fileExtension = Path.GetExtension(request.FileName);
        var chunkIndex = request.ChunkIndex.ToString().PadLeft(request.TotalChunks.ToString().Length, '0');
        var chunkName = $"{correlationId}.{chunkIndex}{fileExtension}";
        var chunkPath = Path.Combine(chunkDirectoryPath, chunkName);
        
        {
            await using var chunkStream = new FileStream(chunkPath, FileMode.Create, FileAccess.Write, FileShare.None);
            await request.Chunk.CopyToAsync(chunkStream, cancellationToken);
        }

        var chunkNameRegex = new Regex($@".*{correlationId}\.\d{{{request.TotalChunks.ToString().Length}}}\..+");
        var chunkPaths = Directory
            .GetFiles(chunkDirectoryPath)
            .Where(f => chunkNameRegex.IsMatch(f))
            .OrderBy(f => f)
            .ToArray();
        
        if (chunkPaths.Length < request.TotalChunks)
            return ChunkedFileUploadRequestStatus.Partial;
        
        if (!Directory.Exists(targetDirectoryPath))
            Directory.CreateDirectory(targetDirectoryPath);
        
        var targetFileName = request.FileName;
        var targetFullPath = Path.Combine(targetDirectoryPath, targetFileName);
        {
            await using var writableStream = new FileStream(targetFullPath, FileMode.Create, FileAccess.Write, FileShare.None);
        
            foreach (var path in chunkPaths)
            {
                await using var chunkStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
                await chunkStream.CopyToAsync(writableStream, cancellationToken);
            }
        }
        
        var targetMetadataFilePath = Path.Combine(targetDirectoryPath, $"{targetFileName}.metadata");
        if (File.Exists(temporaryMetadataFilePath) && !File.Exists(targetMetadataFilePath))
            File.Copy(temporaryMetadataFilePath, targetMetadataFilePath);
        
        try
        {
            if (Directory.Exists(chunkDirectoryPath))
                Directory.Delete(chunkDirectoryPath, true);
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Failed to delete chunk directory");
        }

        return ChunkedFileUploadRequestStatus.Completed;
    }
}