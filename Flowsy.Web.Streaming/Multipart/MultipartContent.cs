using System.Text.Json;
using System.Text.Json.Serialization;
using Flowsy.Web.Streaming.Resources;
using Microsoft.Extensions.Primitives;

namespace Flowsy.Web.Streaming.Multipart;

/// <summary>
/// Represents the multipart content read from a HTTP request.
/// </summary>
public class MultipartContent : IDisposable, IAsyncDisposable
{
    public MultipartContent(IReadOnlyDictionary<string, StringValues> data, IReadOnlyDictionary<string, MultipartFile> files)
    {
        Data = data;
        Files = files;
    }

    ~MultipartContent()
    {
        Dispose(false);
    }

    private bool _disposed;
    
    /// <summary>
    /// The fields from a HTTP request.
    /// </summary>
    public IReadOnlyDictionary<string, StringValues> Data { get; }
    
    /// <summary>
    /// The files from a HTTP request.
    /// </summary>
    public IReadOnlyDictionary<string, MultipartFile> Files { get; }

    /// <summary>
    /// Returns an object from a field expected to be in JSON format.
    /// </summary>
    /// <param name="field">The name of the field.</param>
    /// <typeparam name="T">The type of object to return.</typeparam>
    /// <returns>An instance of T.</returns>
    public T? DeserializeJsonField<T>(string field)
    {
        if (!Data.ContainsKey(field))
            throw new ArgumentException(Strings.InvalidFormField, nameof(field));

        var json = Data[field].FirstOrDefault();
        return json is not null
            ? JsonSerializer.Deserialize<T>(json, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                PropertyNameCaseInsensitive = true,
                Converters =
                {
                    new JsonStringEnumConverter()
                }
            })
            : default;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    public async ValueTask DisposeAsync()
    {
        await DisposeAsync(true);
        GC.SuppressFinalize(this);
    }

    private void Dispose(bool disposing)
    {
        if (_disposed) return;

        if (disposing)
            foreach (var file in Files.Values)
                file.Dispose();

        _disposed = true;
    }
    
    private async Task DisposeAsync(bool disposing)
    {
        if (_disposed) return;

        if (disposing)
            foreach (var file in Files.Values)
                await file.DisposeAsync();

        _disposed = true;
    }
}
