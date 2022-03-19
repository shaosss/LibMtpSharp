using System;
using LibMtpSharp.Structs;

namespace LibMtpSharp.Lists
{
    internal class TrackList : UnmanagedList<TrackStruct>
    {
        public TrackList(IntPtr mptDeviceStructPointer) 
            : base(NativeAPI.LibMtpLibrary.GetTracks(mptDeviceStructPointer))
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