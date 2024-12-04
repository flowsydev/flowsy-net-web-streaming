namespace Flowsy.Web.Streaming.Chunks;

public class ChunkedFileUploadConflictException : Exception
{
    public ChunkedFileUploadConflictException(string message) : base(message)
    {
    }
}