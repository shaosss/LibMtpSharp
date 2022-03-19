using System;
using LibMtpSharp.Structs;

namespace LibMtpSharp.Lists
{
    internal class AlbumList : UnmanagedList<AlbumNativeStruct>
    {
        public AlbumList(IntPtr mptDeviceStructPointer) 
            : base(NativeAPI.LibMtpLibrary.GetAlbums(mptDeviceStructPointer))
        {
        }

        protected override IntPtr GetPointerToNextItem(ref AlbumNativeStruct item)
        {
            return item.next;
        }

        protected override void FreeItem(IntPtr item)
        {
            NativeAPI.LibMtpLibrary.FreeAlbum(item);
        }
    }
}