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
        public uint item_id;
        /// <summary>
        /// ID of parent folder
        /// </summary>
        public uint parent_id;
        /// <summary>
        /// ID of storage holding this track
        /// </summary>
        public uint storage_id;
        /// <summary>
        /// Track title
        /// </summary>
        [MarshalAs(UnmanagedType.LPUTF8Str)]
        public string title;
        /// <summary>
        /// Name of recording artist
        /// </summary>
        [MarshalAs(UnmanagedType.LPUTF8Str)]
        public string artist;
        /// <summary>
        /// Name of recording composer
        /// </summary>
        [MarshalAs(UnmanagedType.LPUTF8Str)]
        public string composer;
        /// <summary>
        /// Genre name for track
        /// </summary>
        [MarshalAs(UnmanagedType.LPUTF8Str)]
        public string genre;
        /// <summary>
        /// Album name for track
        /// </summary>
        [MarshalAs(UnmanagedType.LPUTF8Str)]
        public string album;
        /// <summary>
        /// Date of original recording as a string
        /// </summary>
        [MarshalAs(UnmanagedType.LPUTF8Str)]
        public string date;
        /// <summary>
        /// Original filename of this track
        /// </summary>
        [MarshalAs(UnmanagedType.LPUTF8Str)]
        public string filename;
        /// <summary>
        /// Track number (in sequence on recording)
        /// </summary>
        public ushort tracknumber;
        /// <summary>
        /// Duration in milliseconds
        /// </summary>
        public uint duration;
        /// <summary>
        /// Sample rate of original file, min 0x1f80 max 0xbb80
        /// </summary>
        public uint samplerate;
        /// <summary>
        /// Number of channels in this recording 0 = unknown, 1 or 2
        /// </summary>
        public ushort nochannels;
        /// <summary>
        /// FourCC wave codec name
        /// </summary>
        public uint wavecodec;
        /// <summary>
        /// (Average) bitrate for this file min=1 max=0x16e360 
        /// </summary>
        public uint bitrate;
        /// <summary>
        /// 0 = unused, 1 = constant, 2 = VBR, 3 = free
        /// </summary>
        public ushort bitratetype;
        /// <summary>
        /// User rating 0-100 (0x00-0x64)
        /// </summary>
        public ushort rating;
        /// <summary>
        /// Number of times used/played
        /// </summary>
        public uint usecount;
        /// <summary>
        /// Size of track file in bytes
        /// </summary>
        public ulong filesize;
        /// <summary>
        /// Date of last alteration of the track
        /// </summary>
        public ulong modificationdate;
        /// <summary>
        /// Filetype used for the current track
        /// </summary>
        public FileTypeEnum filetype;
        /// <summary>
        /// Next track in list or NULL if last track
        /// </summary>
        internal IntPtr next;
    }
}