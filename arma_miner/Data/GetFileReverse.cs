using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace arma_miner.Data
{
    public class GetFileReverse : IDisposable
    {
        const int BUFFER_SIZE = 1024;
        private FileStream stream { get; set; }
        private string data { get; set; }
        public Boolean StartOfFile { get; set; }
        private long position { get; set; }
        public GetFileReverse(string filename)
        {
            stream = File.OpenRead(filename);
            if (stream != null)
            {
                position = stream.Seek(0, SeekOrigin.End);
                StartOfFile = false;
                data = string.Empty;
            }
            else
            {
                StartOfFile = true;
            }
        }
        private byte[] ReadStream()
        {
            byte[] bytes = null;
            int size = BUFFER_SIZE;
            if (position != 0)
            {
                bytes = new byte[BUFFER_SIZE];
                long oldPosition = position;
                if (position >= BUFFER_SIZE)
                {
                    position = stream.Seek(-1 * BUFFER_SIZE, SeekOrigin.Current);
                }
                else
                {
                    position = stream.Seek(-1 * position, SeekOrigin.Current);
                    size = (int)(oldPosition - position);
                    bytes = new byte[size];
                }
                stream.Read(bytes, 0, size);
                stream.Seek(-1 * size, SeekOrigin.Current);
            }
            return bytes;

        }
        public string ReadLine()
        {
            string line = "";
            while (!StartOfFile && (!data.Contains("\r\n")))
            {
                byte[] bytes = ReadStream();
                if (bytes != null)
                {
                    string temp = Encoding.UTF8.GetString(bytes);
                    data = data.Insert(0, temp);
                }
                StartOfFile = position == 0;
            }


            int lastReturn = data.LastIndexOf("\r\n");
            if (lastReturn == -1)
            {
                if (data.Length > 0)
                {
                    line = data;
                    data = string.Empty;
                }
                else
                {
                    line = null;
                }
            }
            else
            {
                line = data.Substring(lastReturn + 2);
                data = data.Remove(lastReturn);
            }

            return line;
        }
        public void Close()
        {
            stream.Close();
        }
        public void Dispose()
        {
            stream.Dispose();
            data = string.Empty;
            position = -1;
        }
    }
}
