using System;
using System.Runtime.InteropServices;
using LibMtpSharp.Structs;

namespace LibMtpSharp
{
    public class FileSampleDataPtrHolder : IDisposable
    {
        private IntPtr _fileSampleDataPtr;

        public FileSampleDataPtrHolder(IntPtr devicePtr, uint objectId)
        {
            _fileSampleDataPtr = LibMtpLibrary.CreateFileSampleData();
            if (0 != LibMtpLibrary.GetRepresentativeSample(devicePtr, objectId, _fileSampleDataPtr))
            {
                LibMtpLibrary.DumpErrorStack(devicePtr);
                throw new Exception($"Cannot retrieve representative sample for object id {objectId}");
            }
        }

        public FileSampleDataStruct GetFileSampleDataStruct()
        {
            return Marshal.PtrToStructure<FileSampleDataStruct>(_fileSampleDataPtr);
        }

        private void ReleaseUnmanagedResources()
        {
            if (_fileSampleDataPtr != IntPtr.Zero)
                LibMtpLibrary.FreeFileSampleData(_fileSampleDataPtr);
        }

        public void Dispose()
        {
            ReleaseUnmanagedResources();
            GC.SuppressFinalize(this);
        }

        ~FileSampleDataPtrHolder()
        {
            ReleaseUnmanagedResources();
        }
    }
}