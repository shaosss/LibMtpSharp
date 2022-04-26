using System;
using LibMtpSharp.NativeAPI;
using LibMtpSharp.Structs;

namespace LibMtpSharp.Lists
{
    internal class TrackList : UnmanagedList<TrackStruct>
    {
        public TrackList(IntPtr mptDeviceStructPointer, ProgressFunction? progressCallback) 
            : base(LibMtpLibrary.GetTracks(mptDeviceStructPointer, progressCallback))
        {
        }

        protected override IntPtr GetPointerToNextItem(ref TrackStruct item)
        {
            return item.next;
        }

        protected override void FreeItem(IntPtr item)
        {
            LibMtpLibrary.FreeTrack(item);
        }
    }
}