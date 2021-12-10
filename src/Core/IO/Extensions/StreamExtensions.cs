namespace Aviant.DDD.Core.IO.Extensions;

public static class StreamExtensions
{
    public static byte[] GetAllBytes(this Stream stream)
    {
        using var memoryStream = new MemoryStream();

        stream.CopyTo(memoryStream);
        return memoryStream.ToArray();
    }
}
