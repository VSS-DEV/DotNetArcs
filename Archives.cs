using Ionic.Zlib;
using ICSharpCode.SharpZipLib.BZip2;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.GZip;
using ICSharpCode.SharpZipLib.Tar;
using System.ComponentModel;
using System.IO;
using System.Threading.Tasks;

namespace DotNetArcs
{
    public enum ArchivFormat
    {
        Zlib,
        BZip2,
        GZip,
        // Lzw,
        Tar,
        Zip
    }

    public enum ArchivFunction
    {
        [Description("Производит сжатие данных")]
        Compress,
        [Description("Производит расжатие данных")]
        Decompress
    }

    public sealed class Archives
    {
        public Archives(ArchivFormat archivFormat, ArchivFunction archivFunction, byte[] SomeDataByte, out byte[] SomeArcDataByte) 
        {
            switch (archivFormat)
            {
                case ArchivFormat.Zlib:
                    new ZlibArc(archivFunction, SomeDataByte, out byte[] DataAckZlib);
                    SomeArcDataByte = DataAckZlib;
                    break;
                case ArchivFormat.BZip2:
                    new Bzip2Arc(archivFunction, SomeDataByte, out byte[] DataAckBZip2);
                    SomeArcDataByte = DataAckBZip2;
                    break;
                case ArchivFormat.GZip:
                    new GZipArc(archivFunction, SomeDataByte, out byte[] DataAckGZip);
                    SomeArcDataByte = null;
                    break;
                //case ArchivFormat.Lzw:
                //    SomeArcDataByte = null;
                //    break;
                case ArchivFormat.Tar:
                    new TarArk(archivFunction, SomeDataByte, out byte[] DataAckTar);
                    SomeArcDataByte = null;
                    break;
                case ArchivFormat.Zip:
                    new ZipArc(archivFunction, SomeDataByte, out byte[] DataAckZip);
                    SomeArcDataByte = null;
                    break;
                default:
                    SomeArcDataByte = null;
                    break;
            }
        }
    }

    #region Tar class
    sealed class TarArk
    {
        internal TarArk(ArchivFunction archivFunction, byte[] SomeDataByte, out byte[] SomeArcDataByte)
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

        private byte[] Compressd(byte[] SomeDataByte)
        {
            using (var memoryStream = new MemoryStream())
            {
                var Tsk = new Task(async () => await new TarInputStream(new MemoryStream(SomeDataByte),32,System.Text.Encoding.UTF8).CopyToAsync(memoryStream));
                Tsk.Start();
                Task.WaitAll(Tsk);
                return memoryStream.ToArray();
            }
        }

        private byte[] Decompressd(byte[] SomeDataByte)
        {
            using (var memoryStream = new MemoryStream())
            {
                var Tsk = new Task(async () => await new TarOutputStream(new MemoryStream(SomeDataByte),32,System.Text.Encoding.UTF8).CopyToAsync(memoryStream));
                Tsk.Start();
                Task.WaitAll(Tsk);
                return memoryStream.ToArray();
            }
        }
    }
    #endregion

    #region Lzw class
    //sealed class LzwArc
    //{
    //    internal LzwArc(ArchivFunction archivFunction, byte[] SomeDataByte, out byte[] SomeArcDataByte)
    //    {
    //        switch (archivFunction)
    //        {
    //            case ArchivFunction.Compress:
    //                SomeArcDataByte = Compressd(SomeDataByte);
    //                break;
    //            case ArchivFunction.Decompress:
    //                SomeArcDataByte = Decompressd(SomeDataByte);
    //                break;
    //            default:
    //                SomeArcDataByte = null;
    //                break;
    //        }
    //    }
    //    private byte[] Compressd(byte[] SomeDataByte)
    //    {
    //        using (var memoryStream = new MemoryStream())
    //        {
    //            var Tsk = new Task(async () => await new L(new MemoryStream(SomeDataByte)).CopyToAsync(memoryStream));
    //            Task.WaitAll(Tsk);
    //            return memoryStream.ToArray();
    //        }
    //    }

    //    private byte[] Decompressd(byte[] SomeDataByte)
    //    {
    //        using (var memoryStream = new MemoryStream())
    //        {
    //            var Tsk = new Task(async () => await new GZipInputStream(new MemoryStream(SomeDataByte)).CopyToAsync(memoryStream));
    //            Task.WaitAll(Tsk);
    //            return memoryStream.ToArray();
    //        }
    //    }
    //}
    #endregion

    #region Gzip class
    sealed class GZipArc
    {
        internal GZipArc(ArchivFunction archivFunction, byte[] SomeDataByte, out byte[] SomeArcDataByte)
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

        private byte[] Compressd(byte[] SomeDataByte)
        {
            using (var memoryStream = new MemoryStream())
            {
                var Tsk = new Task(async () => await new GZipOutputStream(new MemoryStream(SomeDataByte)).CopyToAsync(memoryStream));
                Tsk.Start();
                Task.WaitAll(Tsk);
                return memoryStream.ToArray();
            }
        }

        private byte[] Decompressd(byte[] SomeDataByte)
        {
            using (var memoryStream = new MemoryStream())
            {
                var Tsk = new Task(async () => await new GZipInputStream(new MemoryStream(SomeDataByte)).CopyToAsync(memoryStream));
                Tsk.Start();
                Task.WaitAll(Tsk);
                return memoryStream.ToArray();
            }
        }
    }
    #endregion

    #region Zip class
    sealed class ZipArc
    {
        internal ZipArc(ArchivFunction archivFunction, byte[] SomeDataByte, out byte[] SomeArcDataByte)
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

        private byte[] Compressd(byte[] SomeDataByte)
        {
            using (var memoryStream = new MemoryStream())
            {
                var Tsk = new Task(async () => await new ZipOutputStream(new MemoryStream(SomeDataByte)).CopyToAsync(memoryStream));
                Tsk.Start();
                Task.WaitAll(Tsk);
                return memoryStream.ToArray();
            }
        }

        private byte[] Decompressd(byte[] SomeDataByte)
        {
            using (var memoryStream = new MemoryStream())
            {
                var Tsk = new Task(async () => await new ZipInputStream(new MemoryStream(SomeDataByte)).CopyToAsync(memoryStream));
                Tsk.Start();
                Task.WaitAll(Tsk);
                return memoryStream.ToArray();
            }
        }

    }
    #endregion

    #region Bzip2 Class
    sealed class Bzip2Arc
    {
        internal Bzip2Arc(ArchivFunction archivFunction, byte[] SomeDataByte, out byte[] SomeArcDataByte)
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

        private byte[] Compressd(byte[] SomeDataByte)
        {
            using (var memoryStream = new MemoryStream())
            {
                var Tsk = new Task(async() => await new BZip2OutputStream(new MemoryStream(SomeDataByte)).CopyToAsync(memoryStream));
                Tsk.Start();
                Task.WaitAll(Tsk);
                return memoryStream.ToArray();
            }
        }

        private byte[] Decompressd(byte[] SomeDataByte)
        {
            using (var memoryStream = new MemoryStream())
            {
                var Tsk = new Task(async () => await new BZip2InputStream(new MemoryStream(SomeDataByte)).CopyToAsync(memoryStream));
                Tsk.Start();
                Task.WaitAll(Tsk);
                return memoryStream.ToArray();
            }
        }
    }
    #endregion

    #region Zlib Class
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

        #region Синхронные функции
        private byte[] Compressd(byte[] SomeDataByte)
        {
            using (var memoryStream = new MemoryStream())
            {
                var Tsk = new Task(async () => await new ZlibStream(new MemoryStream(SomeDataByte), CompressionMode.Compress, CompressionLevel.Level9).CopyToAsync(memoryStream));
                Tsk.Start();
                Task.WaitAll(Tsk);
                return memoryStream.ToArray();
            }
        }

        private byte[] Decompressd(byte[] SomeDataByte)
        {
            using (var memoryStream = new MemoryStream())
            {
                var Tsk = new Task(async () => await new ZlibStream(new MemoryStream(SomeDataByte), CompressionMode.Decompress).CopyToAsync(memoryStream));
                Tsk.Start();
                Task.WaitAll(Tsk);
                return memoryStream.ToArray();
            }
        }
        #endregion
    }
    #endregion
}
