using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using LibMtpSharp.Enums;
using LibMtpSharp.Exceptions;
using LibMtpSharp.Lists;
using LibMtpSharp.NativeAPI;
using LibMtpSharp.Structs;

namespace LibMtpSharp
{
    public class OpenedMtpDevice : IDisposable
    {
        private readonly IntPtr _mptDeviceStructPointer;
        private readonly bool _cached;

        public OpenedMtpDevice(ref RawDevice rawDevice, bool cached)
        {
            _cached = cached;
            _mptDeviceStructPointer = _cached ? LibMtpLibrary.OpenRawDevice(ref rawDevice) 
                : LibMtpLibrary.OpenRawDeviceUncached(ref rawDevice);
            if (_mptDeviceStructPointer == IntPtr.Zero)
                throw new OpenDeviceException(rawDevice);
        }

        public string? GetManufacturerName()
        {
            return LibMtpLibrary.GetManufacturerName(_mptDeviceStructPointer);
        }

        public string? GetModelName()
        {
            return LibMtpLibrary.GetModelName(_mptDeviceStructPointer);
        }

        public string? GetSerialNumber()
        {
            return LibMtpLibrary.GetSerialNumber(_mptDeviceStructPointer);
        }

        public string? GetDeviceVersion()
        {
            return LibMtpLibrary.GetDeviceVersion(_mptDeviceStructPointer);
        }

        public string? GetFriendlyName()
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
                    yield return fileType;
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
        
        public IEnumerable<FileStruct> GetFolderContent(uint storageId, uint? folderId)
        {
            if (_cached)
                throw new ApplicationException(
                    "GetFolderContent cannot be called on cached device. Open device with cached: false");
            using (var fileList = new FileAndFolderList(_mptDeviceStructPointer, storageId, 
                       folderId ?? LibMtpLibrary.LibmtpFilesAndFoldersRoot))
            {
                foreach (var file in fileList)
                    yield return file;
            }
        }

        public IEnumerable<FileStruct> GetFiles(Func<double, bool>? progressCallback)
        {
            using (var fileList = new FileList(_mptDeviceStructPointer, GetProgressFunction(progressCallback)))
            {
                foreach (var file in fileList)
                {
                    yield return file;
                }
            }
        }

        public IEnumerable<AlbumStruct> GetAlbumList()
        {
            using (var albumList = new AlbumList(_mptDeviceStructPointer))
            {
                foreach (var album in albumList)
                    yield return new AlbumStruct(album);
            }
        }
        
        public IEnumerable<TrackStruct> GetTrackList(Func<double, bool> progressCallback)
        {
            using (var trackList = new TrackList(_mptDeviceStructPointer, GetProgressFunction(progressCallback)))
            {
                foreach (var track in trackList)
                    yield return track;
            }
        }

        public FileSampleDataStruct GetFileSampleDataForObject(uint objectId)
        {
            return new FileSampleDataStruct(_mptDeviceStructPointer, objectId);
        }

        public void SendRepresentativeDataForObject(uint objectId, ref FileSampleDataStruct dataStructStruct)
        {
            dataStructStruct.SendDataToDevice(_mptDeviceStructPointer, objectId);
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

        /// <summary>
        /// Sends the track with passed metadata to the device
        /// </summary>
        /// <param name="track">Track metadata structure to send to device</param>
        /// <param name="dataProvider">Requests data. If data is null - operation considered to be cancelled</param>
        /// <param name="progressCallback">Reports a progress and returns boolean indication if the operation was cancelled</param>
        /// <exception cref="Exception"></exception>
        public void SendTrack(ref TrackStruct track, Func<int, IList<byte>> dataProvider,
            Func<double, bool> progressCallback)
        {
            var result = LibMtpLibrary.SendTrackFromHandler(_mptDeviceStructPointer, GetDataFunction(dataProvider), 
                ref track, GetProgressFunction(progressCallback));
            if (result != 0)
                throw new ApplicationException("Sending file failed");
        }

        /// <summary>
        /// Gets file with specified id to the file on specified path
        /// </summary>
        /// <param name="fileId">id of the file to retrieve</param>
        /// <param name="filePath">file path where to write</param>
        /// <param name="progressCallback">callback for progress reporting</param>
        /// <exception cref="ApplicationException">throws exception if getting the file failed</exception>
        public void GetFile(uint fileId, string filePath, Func<double, bool>? progressCallback)
        {
            var result = LibMtpLibrary.GetFileToFile(_mptDeviceStructPointer, fileId, filePath,
                GetProgressFunction(progressCallback));
            if (result != 0)
                throw new ApplicationException($"Getting file Id: {fileId} to {filePath} failed");
        }

        public void CreateAlbum(ref AlbumStruct albumStruct)
        {
            albumStruct.SendAlbum(_mptDeviceStructPointer, true);
        }
        
        public void UpdateAlbum(ref AlbumStruct albumStruct)
        {
            albumStruct.SendAlbum(_mptDeviceStructPointer, false);
        }

        public uint CreateFolder(string name, uint parentFolderId, uint parentStorageId)
        {
            var newFolderId = LibMtpLibrary.CreateFolder(_mptDeviceStructPointer, name, parentFolderId, parentStorageId);
            if (newFolderId == 0)
                throw new FolderCreationException(name, parentFolderId, parentStorageId);
            return newFolderId;
        }

        public IEnumerable<FolderStruct> GetFolderList(uint? storageId = null)
        {
            using (var folderList = new FolderList(_mptDeviceStructPointer, storageId))
                foreach (var folder in folderList)
                {
                    yield return folder;
                }
        }

        public void SendFirmwareFile(FileInfo fileInfo, Func<double, bool> progressCallback)
        {
            var firmwareFile = new FileStruct
            {
                FileSize = (ulong)fileInfo.Length,
                FileName = fileInfo.Name,
                Filetype = FileTypeEnum.Firmware,
                ParentId = 0,
                StorageId = 0
            };
            LibMtpLibrary.SendFile(_mptDeviceStructPointer, fileInfo.FullName, ref firmwareFile,
                GetProgressFunction(progressCallback), IntPtr.Zero);
        }

        public void DeleteObject(uint objectId)
        {
            if (0 != LibMtpLibrary.DeleteObject(_mptDeviceStructPointer, objectId)) 
                throw new ApplicationException($"Failed to delete the object with it {objectId}");
        }

        private MtpDataGetFunction GetDataFunction(Func<int, IList<byte>?> getData)
        {
            return (IntPtr _, IntPtr _, uint wantlen, IntPtr data, out uint gotlen) =>
            {
                var whereToWrite = data;
                long leftToRead = wantlen;
                gotlen = 0;
                do
                {
                    var howMuchToRead = leftToRead > int.MaxValue ? int.MaxValue : (int)leftToRead;

                    var readBytes = getData(howMuchToRead);
                    if (readBytes == null)
                        return (ushort)HandlerReturn.Cancel;
                    for (int i = 0; i < readBytes.Count; i++)
                    {
                        Marshal.WriteByte(whereToWrite, i, readBytes[i]);
                    }

                    whereToWrite = IntPtr.Add(whereToWrite, readBytes.Count);
                    leftToRead -= readBytes.Count;
                    gotlen += (uint)readBytes.Count;
                } while (leftToRead != 0);

                return (ushort)HandlerReturn.Ok;
            };
        }
        
        private ProgressFunction? GetProgressFunction(Func<double, bool>? progressCallback)
        {
            if (progressCallback == null)
                return null;
            return (sent, total, _) =>
            {
                double progress = (double)sent / total;
                var isCancelled = progressCallback(progress);
                return isCancelled ? 1 : 0;
            };
        }
    }
}