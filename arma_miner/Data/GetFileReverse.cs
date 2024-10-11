using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace arma_miner.Data
{
    public class GetFileReverse : IDisposable
    {
        private const int BUFFER_SIZE = 64 * 1024; // Increased buffer size
        private readonly FileStream _stream;
        private readonly byte[] _buffer;
        private long _position;
        private Memory<byte> _remainingData;

        public GetFileReverse(string filename)
        {
            _stream = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read, BUFFER_SIZE, FileOptions.SequentialScan);
            _buffer = new byte[BUFFER_SIZE];
            _position = _stream.Length;
        }

        public IEnumerable<string> ReadLine()
        {
            var lineEndingBytes = Encoding.UTF8.GetBytes("\n");
            var decoder = Encoding.UTF8.GetDecoder();

            while (_position > 0)
            {
                int bytesToRead = (int)Math.Min(BUFFER_SIZE, _position);
                _position -= bytesToRead;

                _stream.Position = _position;
                int bytesRead = _stream.Read(_buffer, 0, bytesToRead);

                var currentChunk = new Memory<byte>(_buffer, 0, bytesRead);

                if (!_remainingData.IsEmpty)
                {
                    var combinedData = new byte[_remainingData.Length + currentChunk.Length];
                    currentChunk.CopyTo(combinedData);
                    _remainingData.CopyTo(combinedData.AsMemory(_remainingData.Length));
                    currentChunk = combinedData;
                    _remainingData = Memory<byte>.Empty;
                }

                int startIndex = currentChunk.Length;

                for (int i = currentChunk.Length - 1; i >= 0; i--)
                {
                    if (currentChunk.Span[i] == lineEndingBytes[0])
                    {
                        var line = currentChunk.Slice(i + 1, startIndex - i - 1);
                        if (line.Length > 0)
                        {
                            yield return Encoding.UTF8.GetString(line.Span);
                        }
                        startIndex = i;
                    }
                }

                if (startIndex > 0)
                {
                    _remainingData = currentChunk.Slice(0, startIndex);
                }
            }

            if (!_remainingData.IsEmpty)
            {
                yield return Encoding.UTF8.GetString(_remainingData.Span);
            }
        }

        public void Dispose()
        {
            _stream.Dispose();
        }

        //const int BUFFER_SIZE = 1024;
        //private FileStream stream { get; set; }
        //private string data { get; set; }
        //public Boolean StartOfFile { get; set; }
        //private long position { get; set; }
        //public GetFileReverse(string filename)
        //{
        //    stream = File.OpenRead(filename);
        //    if (stream != null)
        //    {
        //        position = stream.Seek(0, SeekOrigin.End);
        //        StartOfFile = false;
        //        data = string.Empty;
        //    }
        //    else
        //    {
        //        StartOfFile = true;
        //    }
        //}
        //private byte[] ReadStream()
        //{
        //    byte[] bytes = null;
        //    int size = BUFFER_SIZE;
        //    if (position != 0)
        //    {
        //        bytes = new byte[BUFFER_SIZE];
        //        long oldPosition = position;
        //        if (position >= BUFFER_SIZE)
        //        {
        //            position = stream.Seek(-1 * BUFFER_SIZE, SeekOrigin.Current);
        //        }
        //        else
        //        {
        //            position = stream.Seek(-1 * position, SeekOrigin.Current);
        //            size = (int)(oldPosition - position);
        //            bytes = new byte[size];
        //        }
        //        stream.Read(bytes, 0, size);
        //        stream.Seek(-1 * size, SeekOrigin.Current);
        //    }
        //    return bytes;

        //}
        //public string ReadLine()
        //{
        //    string line = "";
        //    while (!StartOfFile && (!data.Contains("\r\n")))
        //    {
        //        byte[] bytes = ReadStream();
        //        if (bytes != null)
        //        {
        //            string temp = Encoding.UTF8.GetString(bytes);
        //            data = data.Insert(0, temp);
        //        }
        //        StartOfFile = position == 0;
        //    }


        //    int lastReturn = data.LastIndexOf("\r\n");
        //    if (lastReturn == -1)
        //    {
        //        if (data.Length > 0)
        //        {
        //            line = data;
        //            data = string.Empty;
        //        }
        //        else
        //        {
        //            line = null;
        //        }
        //    }
        //    else
        //    {
        //        line = data.Substring(lastReturn + 2);
        //        data = data.Remove(lastReturn);
        //    }

        //    return line;
        //}
        //public void Close()
        //{
        //    stream.Close();
        //}
        //public void Dispose()
        //{
        //    stream.Dispose();
        //    data = string.Empty;
        //    position = -1;
        //}
    }
}
