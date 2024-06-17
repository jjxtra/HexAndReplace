using System;
using System.IO;
using System.Linq;

namespace HexAndReplace;

/// <summary>
/// Find/replace binary data in a seekable stream
/// </summary>
/// <remarks>
/// Constructor
/// </remarks>
/// <param name="stream">Stream</param>
/// <param name="bufferSize">Buffer size</param>
public sealed class BinaryReplacer(Stream stream, int bufferSize = ushort.MaxValue) : IDisposable
{
    private readonly Stream stream = stream;
    private readonly int bufferSize = bufferSize < 2 ? throw new ArgumentOutOfRangeException(nameof(bufferSize)) : bufferSize;

    /// <inheritdoc />
    public void Dispose()
    {
        stream.Dispose();
    
    }
    /// <summary>
    /// Find and replace binary data in a stream
    /// </summary>
    /// <param name="find">Find</param>
    /// <param name="replace">Replace</param>
    /// <returns>First index of replaced data, or -1 if find is not found</returns>
    /// <exception cref="ArgumentException">Find and replace are not the same length</exception>
    public long Replace(byte[] find, byte[] replace)
    {
        if (find.Length != replace.Length)
        {
            throw new ArgumentException("Find and replace hex must be same length");
        }
        long position = 0;
        long foundPosition = -1;
        byte[] buffer = new byte[bufferSize + find.Length - 1];
        int bytesRead;
        stream.Position = 0;
        while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
        {
            for (int i = 0; i <= bytesRead - find.Length; i++)
            {
                for (int j = 0; j < find.Length; j++)
                {
                    if (buffer[i + j] != find[j]) break;
                    else if (j == find.Length - 1)
                    {
                        stream.Seek(position + i, SeekOrigin.Begin);
                        stream.Write(replace, 0, replace.Length);
                        if (foundPosition == -1)
                        {
                            foundPosition = position + i;
                        }
                        break;
                    }
                }
            }
            position += bytesRead - find.Length + 1;
            if (position > stream.Length - find.Length)
            {
                break;
            }
            stream.Seek(position, SeekOrigin.Begin);
        }
        return foundPosition;
    }
}
