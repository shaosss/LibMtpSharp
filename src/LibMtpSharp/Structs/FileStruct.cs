using System;
using System.Runtime.InteropServices;
using LibMtpSharp.Enums;

namespace LibMtpSharp.Structs
{
    /// <summary>
    /// MTP file struct
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct FileStruct
    {
        /// <summary>
        /// Unique item ID
        /// </summary>
        public uint item_id;
        
        /// <summary>
        /// ID of parent folder
        /// </summary>
        public uint parent_id;
        
        /// <summary>
        /// ID of storage holding this file
        /// </summary>
        public uint storage_id;
        
        /// <summary>
        /// Filename of this file
        /// </summary>
        [MarshalAs(UnmanagedType.LPUTF8Str)]
        public string filename;
        
        /// <summary>
        /// Size of file in bytes
        /// </summary>
        public ulong filesize;
        
        //Todo: check long fits
        /// <summary>
        /// Date of last alteration of the file
        /// </summary>
        public long modificationdate;
        
        /// <summary>
        /// Filetype used for the current file
        /// </summary>
        public FileTypeEnum filetype;
        
        /// <summary>
        ///  Next file in list or NULL if last file 
        /// </summary>
        internal IntPtr next;
    }
}