using System;
using LibMtpSharp.Structs;

namespace LibMtpSharp
{
    public class TrackList : LibMtpUnmanagedList<TrackStruct>
    {
        public TrackList(IntPtr mptDeviceStructPointer) 
            : base(LibMtpLibrary.GetTracks(mptDeviceStructPointer))
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