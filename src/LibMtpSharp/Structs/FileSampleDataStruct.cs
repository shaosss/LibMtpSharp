using System;
using System.Runtime.InteropServices;
using LibMtpSharp.Enums;

namespace LibMtpSharp.Structs
{
    public struct FileSampleDataStruct
    {
        public FileTypeEnum FileType { get; set; }
        public uint Width { get; set; }
        public uint Height { get; set; }
        public uint Duration { get; set; }
        public byte[] Data { get; set; }

        internal FileSampleDataStruct(IntPtr devicePtr, uint objectId)
        {
            var fileSampleDataPtr = NativeAPI.LibMtpLibrary.CreateFileSampleData();
            if (fileSampleDataPtr == IntPtr.Zero)
                throw new ApplicationException("Fails to allocate FileSampleData in LibMtp native layer");
            try
            {
                if (0 != NativeAPI.LibMtpLibrary.GetRepresentativeSample(devicePtr, objectId, fileSampleDataPtr))
                {
                    NativeAPI.LibMtpLibrary.DumpErrorStack(devicePtr);
                    throw new ApplicationException($"Cannot retrieve representative sample for object id {objectId}");
                }
                var fileSampleDataStruct = Marshal.PtrToStructure<FileSampleDataNativeStruct>(fileSampleDataPtr);
                FileType = fileSampleDataStruct.filetype;
                Width = fileSampleDataStruct.width;
                Height = fileSampleDataStruct.height;
                Duration = fileSampleDataStruct.duration;
                Data = fileSampleDataStruct.size == 0 ? null : new byte[fileSampleDataStruct.size];
                var dataPtr = fileSampleDataStruct.data;
                for (ulong i = 0; i < fileSampleDataStruct.size; i++)
                {
                    var insideBlockOffset = (int)(i % int.MaxValue);
                    if (insideBlockOffset == 0 && i != 0)
                        dataPtr = IntPtr.Add(dataPtr, int.MaxValue);
                    Data[i] = Marshal.ReadByte(dataPtr, insideBlockOffset);
                }
            }
            finally
            {
                NativeAPI.LibMtpLibrary.FreeFileSampleData(fileSampleDataPtr);
            }
        }

        internal void SendDataToDevice(IntPtr mptDeviceStructPointer, uint objectId)
        {
            GCHandle handle = GCHandle.Alloc(Data, GCHandleType.Pinned);
            try
            {
                IntPtr dataPtr = handle.AddrOfPinnedObject();
                var fileSampleData = new FileSampleDataNativeStruct()
                {
                    filetype = FileType,
                    height = Height,
                    width = Width,
                    duration = Duration,
                    data = dataPtr,
                    size = (ulong)Data.Length
                };
                NativeAPI.LibMtpLibrary.SendRepresentativeSample(mptDeviceStructPointer, objectId, ref fileSampleData);
            }
            finally
            {
                handle.Free();
            }
        }
    }
}