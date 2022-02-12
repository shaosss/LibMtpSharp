using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using LibMtpSharp.Enums;
using LibMtpSharp.Exceptions;
using LibMtpSharp.Structs;
using Zunc.LibMtp.Exceptions;

namespace LibMtpSharp
{
    public class OpenedMtpDevice : IDisposable
    {
        private readonly IntPtr _mptDeviceStructPointer;

        public OpenedMtpDevice(ref RawDevice rawDevice, bool cached)
        {
            _mptDeviceStructPointer = cached ? LibMtpLibrary.OpenRawDevice(ref rawDevice) 
                : LibMtpLibrary.OpenRawDeviceUncached(ref rawDevice);
            if (_mptDeviceStructPointer == IntPtr.Zero)
                throw new OpenDeviceException(rawDevice);
        }

        public void ForceCachedFlag(bool cached)
        {
            //Hack to enumerate files and folders, while being able to send data to device
            var deviceModifiedStruct = Marshal.PtrToStructure<MtpDeviceStruct>(_mptDeviceStructPointer);
            deviceModifiedStruct.cached = cached ? 1 : 0;
            Marshal.StructureToPtr(deviceModifiedStruct, _mptDeviceStructPointer, false);
        }

        public string GetManufacturerName()
        {
            return LibMtpLibrary.GetManufacturerName(_mptDeviceStructPointer);
        }

        public string GetModelName()
        {
            return LibMtpLibrary.GetModelName(_mptDeviceStructPointer);
        }

        public string GetSerialNumber()
        {
            return LibMtpLibrary.GetSerialNumber(_mptDeviceStructPointer);
        }

        public string GetDeviceVersion()
        {
            return LibMtpLibrary.GetDeviceVersion(_mptDeviceStructPointer);
        }

        public string GetFriendlyName()
        {
            return LibMtpLibrary.GetFriendlyName(_mptDeviceStructPointer);
        }
        
        public void SetFriendlyName(string value)
        {
            throw new NotSupportedException();
        }

        public IEnumerable<FileTypeEnum> GetSupportedTypes()
        {
            using (var fileTypeList = new SupportedTypesList(_mptDeviceStructPointer))
            {
                foreach (var fileType in fileTypeList)
                {
                    yield return fileType;
                }
            }
        }

        public float GetBatteryLevel(bool throwException)
        {
            byte maxBattery = 255, currentBattery = 0; 
            var batterLevel = LibMtpLibrary.GetBatteryLevel(_mptDeviceStructPointer, ref maxBattery, ref currentBattery);
            if (batterLevel != 0)
                return throwException ? throw new ApplicationException() : 0.0f;
            return 100.0f * currentBattery / maxBattery;
        }

        private MtpDeviceStruct CurrentMtpDeviceStruct =>
            Marshal.PtrToStructure<MtpDeviceStruct>(_mptDeviceStructPointer);

        public void PopulateStorages()
        {
            if (LibMtpLibrary.PopulateStorages(_mptDeviceStructPointer) != 0)
                throw new PopulateStoragesException(this);
        }
        
        public IEnumerable<DeviceStorageStruct> GetStorages()
        {
            var deviceStorage = Marshal.PtrToStructure<DeviceStorageStruct>(CurrentMtpDeviceStruct.storage);
            yield return deviceStorage;
            while (deviceStorage.next != IntPtr.Zero)
            {
                deviceStorage = Marshal.PtrToStructure<DeviceStorageStruct>(deviceStorage.next);
                yield return deviceStorage;
            }
        }
        
        public FileAndFolderList GetFolderContent(uint storageId, uint folderId)
        {
            return new(_mptDeviceStructPointer, storageId, folderId);
        }

        public AlbumList GetAlbumList()
        {
            return new(_mptDeviceStructPointer);
        }
        
        public TrackList GetTrackList()
        {
            return new(_mptDeviceStructPointer);
        }

        public FileSampleDataPtrHolder GetFileSampleDataForObject(uint objectId)
        {
            return new FileSampleDataPtrHolder(_mptDeviceStructPointer, objectId);
        }

        public void SendRepresentativeDataForObject(uint objectId, ref FileSampleDataStruct dataStruct)
        {
            LibMtpLibrary.SendRepresentativeSample(_mptDeviceStructPointer, objectId, ref dataStruct);
        }

        private void ReleaseUnmanagedResources()
        {
            if (_mptDeviceStructPointer != IntPtr.Zero)
                LibMtpLibrary.ReleaseDevice(_mptDeviceStructPointer);
        }

        public void Dispose()
        {
            ReleaseUnmanagedResources();
            GC.SuppressFinalize(this);
        }

        ~OpenedMtpDevice()
        {
            ReleaseUnmanagedResources();
        }

        public void SendTrack(ref TrackStruct track, MtpDataGetFunction dataGetFunc,
            ProgressFunction progressFunction)
        {
            var result = LibMtpLibrary.SendTrackFromHandler(_mptDeviceStructPointer, dataGetFunc, ref track, progressFunction);
            if (result != 0)
                throw new Exception("Sending file failed");
        }

        public void CreateAlbum(ref AlbumStruct albumStruct)
        {
            LibMtpLibrary.CreateNewAlbum(_mptDeviceStructPointer, ref albumStruct);
        }
        
        public void UpdateAlbum(ref AlbumStruct albumStruct)
        {
            LibMtpLibrary.UpdateAlbum(_mptDeviceStructPointer, ref albumStruct);
        }

        public uint CreateFolder(string name, uint parentFolderId, uint parentStorageId)
        {
            var newFolderId = LibMtpLibrary.CreateFolder(_mptDeviceStructPointer, name, parentFolderId, parentStorageId);
            if (newFolderId == 0)
                throw new FolderCreationException(name, parentFolderId, parentStorageId);
            return newFolderId;
        }

        public void SendFirmwareFile(FileInfo fileInfo, Action<double> progressCallback)
        {
            var firmwareFile = new FileStruct
            {
                filesize = (ulong)fileInfo.Length,
                filename = fileInfo.Name,
                filetype = FileTypeEnum.Firmware,
                parent_id = 0,
                storage_id = 0
            };
            LibMtpLibrary.SendFile(_mptDeviceStructPointer, fileInfo.FullName, ref firmwareFile,
                (sent, total, _) =>
                {
                    progressCallback((double)sent * 100 / total);
                    return 0;
                }, IntPtr.Zero);
        }

        public void DeleteObject(uint objectId)
        {
            if (0 != LibMtpLibrary.DeleteObject(_mptDeviceStructPointer, objectId)) 
                throw new Exception($"Failed to delete the object with it {objectId}");
        }
    }
}