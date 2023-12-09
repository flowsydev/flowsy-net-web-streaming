# Flowsy Web Streaming

This package includes some services to use buffering when processing multipart requests.

## Configure Services

```csharp
var builder = WebApplication.CreateBuilder(args);

// This step is optional, if no options are configured, BufferingProvider will use default values
builder.Services.Configure<FileBufferingOptions>(options =>
{
    // Set values according to your specific use case
    // options.MemoryThreshold = 0; 
    // options.BufferLimit = 0;
    // options.TempFileDirectory = "";
    // options.TempFileDirectoryAccessor = () => "";
    // options.BytePool = ArrayPool<byte>.Shared;
});

// Configure buffering
builder.Services.AddSingleton<IBufferingProvider, BufferingProvider>();

// Optionally, configure a content inspector with classes from the Flowsy.Content package
builder.Services.AddSingleton<IContentInspector, BasicContentInspector>();

// Configure the the multipart handler
builder.Services.AddSingleton<MultipartHandler>(serviceProvider =>
{
    return new MultipartHandler(
        serviceProvider.GetService<IBufferingProvider>(),
        serviceProvider.GetService<IContentInspector>(),
        serviceProvider.GetService<IConfiguration>()?
            .GetSection("SomeConfiguration:AllowedMimeTypes")
            .GetChildren()
            .Select(c => c.Value!)
        );
});

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
    private readonly MultipartHandler _multipartHandler;

    public DocumentUploadJobController(MultipartHandler multipartHandler)
    {
        _multipartHandler = multipartHandler;
    }
    
    [HttpPost]
    [DisableFormValueModelBinding]
    public async Task<IActionResult> UploadAsync(CancellationToken cancellationToken)
    {
        await using var multipartContent = await _multipartHandler.GetContentAsync(Request, cancellationToken);
        
        // The multipartContent.Data dictionary will contain the data fields from the request
        // The multipartContent.Files dictionary will contain the file fields from the request
        
        return Ok();
    }
}
```