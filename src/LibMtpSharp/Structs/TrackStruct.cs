using System;
using System.Runtime.InteropServices;
using LibMtpSharp.Enums;

namespace LibMtpSharp.Structs
{
    /// <summary>
    /// MTP track struct
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct TrackStruct
    {
        /// <summary>
        /// Unique item ID
        /// </summary>
        public uint ItemId;
        /// <summary>
        /// ID of parent folder
        /// </summary>
        public uint ParentId;
        /// <summary>
        /// ID of storage holding this track
        /// </summary>
        public uint StorageId;
        /// <summary>
        /// Track title
        /// </summary>
        [MarshalAs(UnmanagedType.LPUTF8Str)]
        public string Title;
        /// <summary>
        /// Name of recording artist
        /// </summary>
        [MarshalAs(UnmanagedType.LPUTF8Str)]
        public string Artist;
        /// <summary>
        /// Name of recording composer
        /// </summary>
        [MarshalAs(UnmanagedType.LPUTF8Str)]
        public string Composer;
        /// <summary>
        /// Genre name for track
        /// </summary>
        [MarshalAs(UnmanagedType.LPUTF8Str)]
        public string Genre;
        /// <summary>
        /// Album name for track
        /// </summary>
        [MarshalAs(UnmanagedType.LPUTF8Str)]
        public string Album;
        /// <summary>
        /// Date of original recording as a string
        /// </summary>
        [MarshalAs(UnmanagedType.LPUTF8Str)]
        public string Date;
        /// <summary>
        /// Original filename of this track
        /// </summary>
        [MarshalAs(UnmanagedType.LPUTF8Str)]
        public string FileName;
        /// <summary>
        /// Track number (in sequence on recording)
        /// </summary>
        public ushort TrackNumber;
        /// <summary>
        /// Duration in milliseconds
        /// </summary>
        public uint Duration;
        /// <summary>
        /// Sample rate of original file, min 0x1f80 max 0xbb80
        /// </summary>
        public uint SampleRate;
        /// <summary>
        /// Number of channels in this recording 0 = unknown, 1 or 2
        /// </summary>
        public ushort NumberOfChannels;
        /// <summary>
        /// FourCC wave codec name
        /// </summary>
        public uint Wavecodec;
        /// <summary>
        /// (Average) bitrate for this file min=1 max=0x16e360 
        /// </summary>
        public uint Bitrate;
        /// <summary>
        /// 0 = unused, 1 = constant, 2 = VBR, 3 = free
        /// </summary>
        public ushort BitrateType;
        /// <summary>
        /// User rating 0-100 (0x00-0x64)
        /// </summary>
        public ushort Rating;
        /// <summary>
        /// Number of times used/played
        /// </summary>
        public uint Usecount;
        /// <summary>
        /// Size of track file in bytes
        /// </summary>
        public ulong Filesize;
        /// <summary>
        /// Date of last alteration of the track
        /// </summary>
        public ulong ModificationDate;
        /// <summary>
        /// Filetype used for the current track
        /// </summary>
        public FileTypeEnum FileType;
        /// <summary>
        /// Next track in list or NULL if last track
        /// </summary>
        internal IntPtr next;
    }
}