using System;
using System.Runtime.InteropServices;

namespace LibMtpSharp.Structs
{
    /// <summary>
    /// LIBMTP Device Storage structure
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct DeviceStorageStruct
    {
        /// <summary>
        /// Unique ID for this storage
        /// </summary>
        public uint id;
        
        /// <summary>
        /// Storage type
        /// </summary>
        ushort StorageType;
        
        /// <summary>
        /// Filesystem type
        /// </summary>
        ushort FilesystemType;
        
        /// <summary>
        ///  Access capability
        /// </summary>
        ushort AccessCapability;
        
        /// <summary>
        /// Maximum capability
        /// </summary>
        public ulong MaxCapacity;
        
        /// <summary>
        /// Free space in bytes
        /// </summary>
        public ulong FreeSpaceInBytes;
        
        /// <summary>
        /// Free space in objects
        /// </summary>
        public ulong FreeSpaceInObjects;
        
        /// <summary>
        /// A brief description of this storage
        /// </summary>
        [MarshalAs(UnmanagedType.LPUTF8Str)]
        public string StorageDescription;
        
        /// <summary>
        /// A volume identifier
        /// </summary>
        [MarshalAs(UnmanagedType.LPUTF8Str)]
        public string VolumeIdentifier;
        
        /// <summary>
        /// Next storage, follow this link until NULL
        /// </summary>
        internal IntPtr next;
        
        /// <summary>
        /// Previous storage
        /// </summary>
        IntPtr prev;
    }
}