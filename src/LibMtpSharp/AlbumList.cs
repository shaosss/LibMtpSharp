using System;
using LibMtpSharp.Structs;

namespace LibMtpSharp
{
    public class AlbumList : LibMtpUnmanagedList<AlbumStruct>
    {
        public AlbumList(IntPtr mptDeviceStructPointer) 
            : base(LibMtpLibrary.GetAlbums(mptDeviceStructPointer))
        {
        }

        protected override IntPtr GetPointerToNextItem(ref AlbumStruct item)
        {
            return item.next;
        }

        protected override void FreeItem(IntPtr item)
        {
            LibMtpLibrary.FreeAlbum(item);
        }
    }
}