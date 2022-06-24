using Ionic.Zlib;
using ICSharpCode.SharpZipLib.BZip2;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.GZip;
using ICSharpCode.SharpZipLib.Tar;
using System.ComponentModel;
using System.IO;
using System;

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
        [Description("Производит разжатие данных")]
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
        private byte[] tmpData { get; set; }
        internal TarArk(ArchivFunction archivFunction, byte[] SomeDataByte, out byte[] SomeArcDataByte)
        {
            switch (archivFunction)
            {
                case ArchivFunction.Compress:
                    Compressd(SomeDataByte);
                    SomeArcDataByte = tmpData; 
                    break;
                case ArchivFunction.Decompress:
                    Decompressd(SomeDataByte);
                    SomeArcDataByte = tmpData; 
                    break;
                default:
                    SomeArcDataByte = null;
                    break;
            }
        }

        private void Compressd(byte[] SomeDataByte)
        {
            using (var memoryStream = new MemoryStream())
            {
                new TarInputStream(new MemoryStream(SomeDataByte),32,System.Text.Encoding.UTF8).CopyTo(memoryStream);
                tmpData = memoryStream.ToArray();
            }
        }

        private void Decompressd(byte[] SomeDataByte)
        {
            using (var memoryStream = new MemoryStream())
            {
                new TarOutputStream(new MemoryStream(SomeDataByte),32,System.Text.Encoding.UTF8).CopyTo(memoryStream);
                tmpData = memoryStream.ToArray();
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
        private byte[] tmpData { get; set; }
        internal GZipArc(ArchivFunction archivFunction, byte[] SomeDataByte, out byte[] SomeArcDataByte)
        {
            switch (archivFunction)
            {
                case ArchivFunction.Compress:
                    Compressd(SomeDataByte);
                    SomeArcDataByte = tmpData; 
                    break;
                case ArchivFunction.Decompress:
                    Decompressd(SomeDataByte);
                    SomeArcDataByte = tmpData; 
                    break;
                default:
                    SomeArcDataByte = null;
                    break;
            }
        }

        private void Compressd(byte[] SomeDataByte)
        {
            using (var memoryStream = new MemoryStream())
            {
                new GZipOutputStream(new MemoryStream(SomeDataByte)).CopyTo(memoryStream);
                tmpData = memoryStream.ToArray();
            }
        }

        private void Decompressd(byte[] SomeDataByte)
        {
            using (var memoryStream = new MemoryStream())
            {
                new GZipInputStream(new MemoryStream(SomeDataByte)).CopyTo(memoryStream);
                tmpData=memoryStream.ToArray();
            }
        }
    }
    #endregion

    #region Zip class
    sealed class ZipArc
    {
        private byte[] tmpData { get; set; }
        internal ZipArc(ArchivFunction archivFunction, byte[] SomeDataByte, out byte[] SomeArcDataByte)
        {
            switch (archivFunction)
            {
                case ArchivFunction.Compress:
                    Compressd(SomeDataByte);
                    SomeArcDataByte = tmpData; 
                    break;
                case ArchivFunction.Decompress:
                    Decompressd(SomeDataByte);
                    SomeArcDataByte = tmpData; 
                    break;
                default:
                    SomeArcDataByte = null;
                    break;
            }
        }

        private void Compressd(byte[] SomeDataByte)
        {
            using (var memoryStream = new MemoryStream())
            {
                new ZipOutputStream(new MemoryStream(SomeDataByte)).CopyTo(memoryStream);
                tmpData = memoryStream.ToArray();
            }
        }

        private void Decompressd(byte[] SomeDataByte)
        {
            using (var memoryStream = new MemoryStream())
            {
               new ZipInputStream(new MemoryStream(SomeDataByte)).CopyTo(memoryStream);
               tmpData = memoryStream.ToArray();
            }
        }

    }
    #endregion

    #region Bzip2 Class
    sealed class Bzip2Arc
    {
        private byte[] tmpData { get; set; }
        internal Bzip2Arc(ArchivFunction archivFunction, byte[] SomeDataByte, out byte[] SomeArcDataByte)
        {
            switch (archivFunction)
            {
                case ArchivFunction.Compress:
                    Compressd(SomeDataByte);
                    SomeArcDataByte = tmpData; 
                    break;
                case ArchivFunction.Decompress:
                    Decompressd(SomeDataByte);
                    SomeArcDataByte = tmpData;
                    break;
                default:
                    SomeArcDataByte = null;
                    break;
            }
        }

        private void Compressd(byte[] SomeDataByte)
        {
            using (var memoryStream = new MemoryStream())
            {
                new BZip2OutputStream(new MemoryStream(SomeDataByte)).CopyTo(memoryStream);
                tmpData = memoryStream.ToArray();
            }
        }

        private void Decompressd(byte[] SomeDataByte)
        {
            using (var memoryStream = new MemoryStream())
            {
                new BZip2InputStream(new MemoryStream(SomeDataByte)).CopyTo(memoryStream);
                tmpData = memoryStream.ToArray();
            }
        }
    }
    #endregion

    #region Zlib Class
    sealed class ZlibArc
    {
        private byte[] tmpData { get; set; }
        internal ZlibArc(ArchivFunction archivFunction, byte[] SomeDataByte, out byte[] SomeArcDataByte)
        {
            switch (archivFunction)
            {
                case ArchivFunction.Compress:
                    new AsyncTasker(new Action(async () => {
                        using (var memoryStream = new MemoryStream())
                        {
                            await new ZlibStream(new MemoryStream(SomeDataByte), CompressionMode.Compress, CompressionLevel.Level9).CopyToAsync(memoryStream);
                            tmpData = memoryStream.ToArray();
                        }
                    }));

                    SomeArcDataByte = tmpData;
                    break;
                case ArchivFunction.Decompress:
                    new AsyncTasker(new Action(async () => {
                        using (var memoryStream = new MemoryStream())
                        {
                            await new ZlibStream(new MemoryStream(SomeDataByte), CompressionMode.Decompress).CopyToAsync(memoryStream);
                            tmpData = memoryStream.ToArray();
                        }
                    }));

                    SomeArcDataByte = tmpData;
                    break;
                default:
                    SomeArcDataByte = null;
                    break;
            }
        }
        
    }
    #endregion
}
