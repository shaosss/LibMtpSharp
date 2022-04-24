using System;
using LibMtpSharp.NativeAPI;
using LibMtpSharp.Structs;

namespace LibMtpSharp.Lists
{
    internal class TrackList : UnmanagedList<TrackStruct>
    {
        public TrackList(IntPtr mptDeviceStructPointer, ProgressFunction progressCallback) 
            : base(NativeAPI.LibMtpLibrary.GetTracks(mptDeviceStructPointer, progressCallback))
        {
        }

        protected override IntPtr GetPointerToNextItem(ref TrackStruct item)
        {
            return item.next;
        }

        protected override void FreeItem(IntPtr item)
        {
            NativeAPI.LibMtpLibrary.FreeTrack(item);
        }
    }
}