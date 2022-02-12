using System;
using LibMtpSharp.Structs;

namespace LibMtpSharp
{
    public class FileAndFolderList : LibMtpUnmanagedList<FileStruct>
    {
        public FileAndFolderList(IntPtr mptDeviceStructPointer, uint storageId, uint parentId)
            : base(LibMtpLibrary.GetParentContent(mptDeviceStructPointer, storageId, parentId))
        {
        }

        protected override IntPtr GetPointerToNextItem(ref FileStruct item)
        {
            return item.next;
        }

        protected override void FreeItem(IntPtr item)
        {
            LibMtpLibrary.FreeFile(item);
        }
    }
}