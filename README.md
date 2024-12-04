# Flowsy Web Streaming

This package includes some streaming services for web applications.

## Configure Services

```csharp
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddStreaming()
    .UseBuffering((options, serviceProvider) => 
    {
        // Set values according to your specific use case
        // options.MemoryThreshold = someNumber; 
        // options.BufferLimit = someNumber;
        // options.TempFileDirectory = "/path/to/some/directory";
        // options.TempFileDirectoryAccessor = () => "/path/to/some/directory";
        // options.BytePool = ArrayPool<byte>.Shared;    
    })
    .UseMultipartRequests((options, serviceProvider) =>
    {
        // Set values according to your specific use case
        // options.AllowedMimeTypes = new[] { "image/jpeg", "image/png" };
    })
    .UseChunkedFileUploads();

// Configure more services

var app = builder.Build();

// Configure request pipeline

app.Run();
```

## Processing Requests
```csharp
[ApiController]
[Route($"{Routes.Content}/[controller]")]
public class SomeController : ControllerBase
{
    private readonly IMultipartHandler _multipartHandler;

    public SomeController(IMultipartHandler multipartHandler)
    {
        _multipartHandler = multipartHandler;
    }
    
    [HttpPost]
    [DisableFormValueModelBinding]
    public async Task<IActionResult> UploadContentAsync(CancellationToken cancellationToken)
    {
        await using var multipartContent = await _multipartHandler.GetContentAsync(Request, cancellationToken);
        
        // The multipartContent.Data dictionary will contain the data fields from the request
        // The multipartContent.Files dictionary will contain the file fields from the request
        
        return Ok();
    }
}
```

```csharp
[ApiController]
[Route($"{Routes.Content}/[controller]")]
public class AnotherController : ControllerBase
{
    private readonly IChunkedFileUploadHandler _chunkedFileUploadHandler;

    public AnotherController(IChunkedFileUploadHandler chunkedFileUploadHandler)
    {
        _chunkedFileUploadHandler = chunkedFileUploadHandler;
    }
    
    [HttpPost]
    public async Task<IActionResult> UploadChunkedFileAsync(ChunkedFileUploadRequest request, CancellationToken cancellationToken)
    {
        var correlationId = request.CorrelationId;
        
        // Example use case for ChunkedFileUploadRequest.Metadata
        if (request is {ChunkIndex: 0, Metadata: null})
            return BadRequest("Metadata is required for the first chunk.");
        
        var chunkDirectory = "/path/to/chunks/directory";
        var targetDirectory = "/path/to/final/directory";

        var status = status = await _chunkedFileUploadHandler.HandleAsync(request, chunkDirectory, targetDirectory, cancellationToken);
        
        // There are more chunks to upload, return 202 (accepted), so the client can upload the next chunk 
        if (status == ChunkedFileUploadRequestStatus.Partial)
            return Accepted();
        
        // All the chunks were uploaded
        // - The reassembled file was saved as /path/to/final/directory/{request.FileName}
        // - The metadata (if any) was saved as /path/to/final/directory/{request.FileName}.metadata
        
        // The file is fully uploaded, return 200 (ok)
        return Ok();
    }
}
```