using System;
using LibMtpSharp.NativeAPI;
using LibMtpSharp.Structs;

namespace LibMtpSharp.Lists;

internal class FileList : UnmanagedList<FileStruct>
{
    public FileList(IntPtr mptDeviceStructPointer, ProgressFunction? progressCallback) 
        : base(LibMtpLibrary.GetFilelistingWithCallback(mptDeviceStructPointer, progressCallback))
    {
    }

    protected override IntPtr GetPointerToNextItem(ref FileStruct item)
    {
        return item.next;
    }

    protected override void FreeItem(IntPtr item)
    {
        LibMtpLibrary.DestroyFile(item);
    }
}