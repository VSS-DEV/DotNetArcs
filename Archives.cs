using Ionic.Zlib;
using System.ComponentModel;
using System.IO;
using System.Threading.Tasks;

namespace DotNetArcs
{
    public enum ArchivFormat
    {
        Zlib
    }

    public enum ArchivFunction
    {
        [Description("Производит сжатие данных")]
        Compress,
        [Description("Производит растяжение данных")]
        Decompress
    }

    public sealed class Archives
    {
        public Archives(ArchivFormat archivFormat, ArchivFunction archivFunction, byte[] SomeDataByte, out byte[] SomeArcDataByte)
        {
            switch (archivFormat)
            {
                case ArchivFormat.Zlib:
                    new ZlibArc(archivFunction, SomeDataByte, out byte[] DataAck);
                    SomeArcDataByte = DataAck;
                    break;
                default:
                    SomeArcDataByte = null;
                    break;
            }
        }
        public Archives() { }

        public Task<byte[]> ArchivesAsync(ArchivFormat archivFormat, ArchivFunction archivFunction, byte[] SomeDataByte)
        {
            

            switch (archivFormat)
            {
                case ArchivFormat.Zlib:
                    var AsyncZlib = new ZlibArc();
                    return AsyncZlib.ZlibArcAsync(archivFunction, SomeDataByte);
                default:
                    return null;
            }
        }
    }

    sealed class ZlibArc
    {
        internal ZlibArc(ArchivFunction archivFunction, byte[] SomeDataByte, out byte[] SomeArcDataByte)
        {
            switch (archivFunction)
            {
                case ArchivFunction.Compress:
                    SomeArcDataByte = Compressd(SomeDataByte);
                    break;
                case ArchivFunction.Decompress:
                    SomeArcDataByte = Decompressd(SomeDataByte);
                    break;
                default:
                    SomeArcDataByte = null;
                    break;
            }
        }
        internal ZlibArc() { }

        internal async Task<byte[]> ZlibArcAsync(ArchivFunction archivFunction, byte[] SomeDataByte)
        {
            switch (archivFunction)
            {
                case ArchivFunction.Compress:
                    
                    return await CompressdAsync(SomeDataByte);
                case ArchivFunction.Decompress:
                    return await DecompressdAsync(SomeDataByte);
                default:
                    return null;
            }
        }

        #region Асинхронные функции
        private async Task<byte[]> CompressdAsync(byte[] SomeDataByte)
        {
            using (var memoryStream = new MemoryStream())
            {
                var ZlibStream = new ZlibStream(new MemoryStream(SomeDataByte), CompressionMode.Compress, CompressionLevel.Level9);
                await Task.Delay(200);
                await ZlibStream.CopyToAsync(memoryStream);
                return memoryStream.ToArray();
            }
        }

        private async Task<byte[]> DecompressdAsync(byte[] SomeDataByte)
        {
            using (var memoryStream = new MemoryStream())
            {
                var ZlibStram = new ZlibStream(new MemoryStream(SomeDataByte), CompressionMode.Decompress);
                await Task.Delay(200);
                await ZlibStram.CopyToAsync(memoryStream);
                return memoryStream.ToArray();
            }
        }
        #endregion

        #region Синхронные функции
        private byte[] Compressd(byte[] SomeDataByte)
        {
            using (var memoryStream = new MemoryStream())
            {
                new ZlibStream(new MemoryStream(SomeDataByte), CompressionMode.Compress, CompressionLevel.Level9).CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }

        private byte[] Decompressd(byte[] SomeDataByte)
        {
            using (var memoryStream = new MemoryStream())
            {
                new ZlibStream(new MemoryStream(SomeDataByte), CompressionMode.Decompress).CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }
        #endregion
    }
}
