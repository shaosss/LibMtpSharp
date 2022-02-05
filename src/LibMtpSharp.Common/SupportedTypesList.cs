using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using LibMtpSharp.Enums;

namespace LibMtpSharp
{
    internal class SupportedTypesList : IEnumerable<FileTypeEnum>, IDisposable
    {
        private readonly IntPtr _listPointer;
        private readonly ushort _lengthOfList;
        
        public SupportedTypesList(IntPtr mptDeviceStructPointer)
        {
            var error = LibMtpLibrary.GetSupportedFiletypes(mptDeviceStructPointer, ref _listPointer,
                ref _lengthOfList);
            if (error != 0)
                throw new ApplicationException("Error while retrieving supported types");    
        }
        
        public IEnumerator<FileTypeEnum> GetEnumerator()
        {
            for (int i = 0; i < _lengthOfList; i++)
            {
                IntPtr offset = _listPointer + i * Marshal.SizeOf(typeof(ushort));
                var deviceObject = Marshal.PtrToStructure(offset, typeof(ushort));
                yield return (FileTypeEnum) deviceObject!;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private void ReleaseUnmanagedResources()
        {
            LibMtpLibrary.Free(_listPointer);
        }

        public void Dispose()
        {
            ReleaseUnmanagedResources();
            GC.SuppressFinalize(this);
        }
        
        ~SupportedTypesList()
        {
            ReleaseUnmanagedResources();
        }
    }
}