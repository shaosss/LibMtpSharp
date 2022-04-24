using System;
using LibMtpSharp.NativeAPI;
using LibMtpSharp.Structs;

namespace LibMtpSharp.Lists;

internal class FolderList : UnmanagedList<FolderStruct>
{
    private bool _treeWasFreed = false;
    
    public FolderList(IntPtr mptDeviceStructPointer, uint? storageId = null) 
        : base(storageId == null 
            ? LibMtpLibrary.GetFolderList(mptDeviceStructPointer) 
            : LibMtpLibrary.GetFolderListForStorage(mptDeviceStructPointer, storageId.Value))
    {
    }

    protected override IntPtr GetPointerToNextItem(ref FolderStruct item)
    {
        if (_treeWasFreed)
            return IntPtr.Zero;
        if (item.sibling != IntPtr.Zero)
            return item.sibling;
        return item.child;
    }

    protected override void FreeItem(IntPtr item)
    {
        _treeWasFreed = true;
        LibMtpLibrary.DestroyFolder(item);
    }
}