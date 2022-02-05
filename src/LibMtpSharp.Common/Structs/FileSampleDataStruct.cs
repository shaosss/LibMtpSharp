using System;
using System.Runtime.InteropServices;
using LibMtpSharp.Enums;

namespace LibMtpSharp.Structs
{
    [StructLayout(LayoutKind.Sequential)]
    public struct FileSampleDataStruct
    {
        /// <summary>
        /// Width of sample if it is an image
        /// </summary>
        public uint width;
        /// <summary>
        /// Height of sample if it is an image
        /// </summary>
        public uint height;
        /// <summary>
        /// Duration in milliseconds if it is audio
        /// </summary>
        uint duration;
        /// <summary>
        /// Filetype used for the sample
        /// </summary>
        public FileTypeEnum filetype;
        /// <summary>
        /// Size of sample data in bytes
        /// </summary>
        public ulong size;
        /// <summary>
        /// Sample data
        /// </summary>
        public IntPtr data;
    }
}