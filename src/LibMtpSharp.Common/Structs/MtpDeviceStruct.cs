using System;
using System.Runtime.InteropServices;

namespace LibMtpSharp.Structs
{
    /// <summary>
    /// Main MTP device object struct
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal struct MtpDeviceStruct
    {
        /// <summary>
        /// Object bitsize, typically 32 or 64.
        /// </summary>
        byte object_bitsize;

        /// <summary>
        /// Parameters for this device, must be cast into (PTPParams*) before internal use.
        /// </summary>
        IntPtr @params;

        /// <summary>
        /// USB device for this device, must be cast into (PTP_USB*) before internal use.
        /// </summary>
        IntPtr usbinfo;

        /// <summary>
        /// Pointer to <see cref="DeviceStorageStruct"/> The storage for this device, do not use strings in here without copying them first,
        /// and beware that this list may be rebuilt at any time. @see LIBMTP_Get_Storage()
        /// </summary>
        public IntPtr storage;

        /// <summary>
        /// Pointer to ErrorStruct The error stack. This shall be handled using the error getting
        /// and clearing functions, not by dereferencing this list.
        /// </summary>
        IntPtr errorstack;

        /// <summary>
        /// The maximum battery level for this device
        /// </summary>
        byte maximum_battery_level;

        /// <summary>
        /// Default music folder
        /// </summary>
        uint default_music_folder;

        /// <summary>
        /// Default playlist folder
        /// </summary>
        uint default_playlist_folder;

        /// <summary>
        /// Default picture folder
        /// </summary>
        uint default_picture_folder;

        /// <summary>
        /// Default video folder
        /// </summary>
        uint default_video_folder;

        /// <summary>
        /// Default organizer folder
        /// </summary>
        uint default_organizer_folder;

        /// <summary>
        /// Default ZENcast folder (only Creative devices...)
        /// </summary>
        uint default_zencast_folder;

        /// <summary>
        /// Default Album folder
        /// </summary>
        uint default_album_folder;

        /// <summary>
        /// Default Text folder
        /// </summary>
        uint default_text_folder;

        /// <summary>
        /// Per device iconv() converters, only used internally
        /// </summary>
        IntPtr cd;

        /// <summary>
        /// Pointer to <see cref="DeviceExtension"/> Extension list
        /// </summary>
        IntPtr extensions;

        /// <summary>
        /// Whether the device uses caching, only used internally
        /// </summary>
        public int cached;

        /// <summary>
        /// Pointer to next device in linked list; NULL if this is the last device
        /// </summary>
        IntPtr next;
    }
}