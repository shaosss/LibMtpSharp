using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using LibMtpSharp.NativeAPI;
using LibMtpSharp.Structs;

namespace LibMtpSharp.Lists;

internal class FolderList : IEnumerable<FolderStruct>, IDisposable
{
    private readonly IntPtr _listPtr;

    public FolderList(IntPtr mptDeviceStructPointer, uint? storageId = null)
    {
        _listPtr = storageId == null 
            ? LibMtpLibrary.GetFolderList(mptDeviceStructPointer) 
            : LibMtpLibrary.GetFolderListForStorage(mptDeviceStructPointer, storageId.Value);
    }
    
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public IEnumerator<FolderStruct> GetEnumerator()
    {
        var currentItem = _listPtr;
        if (currentItem != IntPtr.Zero)
        {
            foreach (var folder in EnumerateFolderTreeStartingFrom(_listPtr))
                yield return folder;
        }
    }

    private IEnumerable<FolderStruct> EnumerateFolderTreeStartingFrom(IntPtr folderPtr)
    {
        var folder = Marshal.PtrToStructure<FolderStruct>(folderPtr);
        yield return folder;
        if (folder.Child != IntPtr.Zero)
        {
            foreach (var childFolder in EnumerateFolderTreeStartingFrom(folder.Child))
                yield return childFolder;
        }

        if (folder.Sibling != IntPtr.Zero)
        {
            foreach (var siblingFolder in EnumerateFolderTreeStartingFrom(folder.Sibling))
                yield return siblingFolder;
        }
    }

    private void ReleaseUnmanagedResources()
    {
        if (_listPtr != IntPtr.Zero)
            LibMtpLibrary.DestroyFolder(_listPtr);
    }

    public void Dispose()
    {
        ReleaseUnmanagedResources();
        GC.SuppressFinalize(this);
    }
        
    ~FolderList()
    {
        ReleaseUnmanagedResources();
    }
}