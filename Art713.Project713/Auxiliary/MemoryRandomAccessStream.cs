using System;
using System.IO;
using Windows.Foundation;
using Windows.Storage.Streams;

namespace Art713.Project713.Auxiliary
{
    class MemoryRandomAccessStream : IRandomAccessStream
    {
        private Stream _stream;

        public MemoryRandomAccessStream(Stream stream)
        {
            _stream = stream;
        }

        public MemoryRandomAccessStream(byte[] bytes)
        {
            _stream = new MemoryStream(bytes);
        }

        public void Dispose()
        {
            _stream.Dispose();
        }

        public IAsyncOperationWithProgress<IBuffer, uint> ReadAsync(IBuffer buffer, uint count, InputStreamOptions options)
        {
            var inputStream = GetInputStreamAt(0);
            return inputStream.ReadAsync(buffer, count, options);
        }

        public IAsyncOperationWithProgress<uint, uint> WriteAsync(IBuffer buffer)
        {
            var outputStream = GetOutputStreamAt(0);
            return outputStream.WriteAsync(buffer);
        }

        public IAsyncOperation<bool> FlushAsync()
        {
            var outputStream = GetOutputStreamAt(0);
            return outputStream.FlushAsync();
        }

        public IInputStream GetInputStreamAt(ulong position)
        {
            _stream.Seek((long) position, SeekOrigin.Begin);
            return _stream.AsInputStream();
        }

        public IOutputStream GetOutputStreamAt(ulong position)
        {
            _stream.Seek((long)position, SeekOrigin.Begin);
            return _stream.AsOutputStream();
        }

        public void Seek(ulong position)
        {
            _stream.Seek((long)position, 0);
        }

        public IRandomAccessStream CloneStream()
        {
            throw new NotSupportedException();
        }

        public bool CanRead
        {
            get { return true; }
        }

        public bool CanWrite
        {
            get { return true; }
        }

        public ulong Position 
        {
            get { return (ulong)_stream.Position; }
        }

        public ulong Size 
        { 
            get { return (ulong) _stream.Length; }
            set { _stream.SetLength((long)value); }
        }
    }
}
