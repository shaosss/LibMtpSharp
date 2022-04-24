using System;
using System.Runtime.InteropServices;

namespace LibMtpSharp.Structs
{
    /// <summary>
    /// MTP Folder structure
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct FolderStruct
    {
        /// <summary>
        /// Unique folder ID
        /// </summary>
        public uint folder_id;
        
        /// <summary>
        /// ID of parent folder
        /// </summary>
        public uint parent_id;
        
        /// <summary>
        /// ID of storage holding this folder
        /// </summary>
        public uint storage_id;
        
        /// <summary>
        /// Name of folder
        /// </summary>
        [MarshalAs(UnmanagedType.LPUTF8Str)]
        public string name;
        
        /// <summary>
        /// Next folder at same level or NULL if no more
        /// </summary>
        internal IntPtr sibling;
        
        /// <summary>
        /// Child folder or NULL if no children
        /// </summary>
        internal IntPtr child;
    }
}