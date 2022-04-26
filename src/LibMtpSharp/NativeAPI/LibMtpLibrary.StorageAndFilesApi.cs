using System;
using System.Runtime.InteropServices;
using LibMtpSharp.Structs;

namespace LibMtpSharp.NativeAPI
{
    /// <summary>
    /// Storage and files API.
    /// </summary>
    internal partial class LibMtpLibrary
    {
        /// <summary>
        /// This function retrieves a list of supported file types, i.e. the file types that this device claims
        /// it supports, e.g. audio file types that the device can play etc. This list is mitigated to include
        /// the file types that libmtp can handle, i.e. it will not list filetypes that libmtp will handle internally
        /// like playlists and folders.
        /// </summary>
        /// <param name="mtpDeviceStructPointer">a pointer to the device to get the filetype capabilities for.</param>
        /// <param name="filetypes">a pointer to a pointer that will hold the list of supported filetypes if
        /// the call was successful. This list must be <code>free()</code>:ed by the caller after use.</param>
        /// <param name="length">a pointer to a variable that will hold the length of the list of supported filetypes
        /// if the call was successful.</param>
        /// <returns>0 on success, any other value means failure.</returns>
        [DllImport(LibMtpName)]
        private static extern int LIBMTP_Get_Supported_Filetypes(IntPtr mtpDeviceStructPointer, ref IntPtr filetypes, ref ushort length);

        public static int GetSupportedFiletypes(IntPtr mtpDeviceStructPointer, ref IntPtr filetypes, ref ushort length)
        {
            return LIBMTP_Get_Supported_Filetypes(mtpDeviceStructPointer, ref filetypes, ref length);
        }


        /// <summary>
        ///This function updates all the storage id's of a device and their properties, then creates a linked list
        /// and puts the list head into the device struct. It also optionally sorts this list. If you want to display
        /// storage information in your application you should call this function, then dereference the device struct
        /// (device->storage) to get out information on the storage.
        /// You need to call this every time you want to update the device->storage list, for example anytime you need
        /// to check available storage somewhere.
        /// 
        /// <b>WARNING:</b> since this list is dynamically updated, do not reference its fields in external applications
        /// by pointer! E.g do not put a reference to any char * field. instead strncpy() it!
        /// </summary>
        /// <param name="mtpDeviceStructPointer">device a pointer to the device to get the storage for.</param>
        /// <param name="sortby">sortby an integer that determines the sorting of the storage list.
        /// Valid sort methods are defined in libmtp.h with beginning with LIBMTP_STORAGE_SORTBY_. 0
        /// or LIBMTP_STORAGE_SORTBY_NOTSORTED to not sort.</param>
        /// <returns>0 on success, 1 success but only with storage id's, storage properities could not be retrieved
        /// and -1 means failure.</returns>
        [DllImport(LibMtpName)]
        private static extern int LIBMTP_Get_Storage(IntPtr mtpDeviceStructPointer, int sortby);
        
        public static int PopulateStorages(IntPtr mtpDeviceStructPointer)
        {
            return LIBMTP_Get_Storage(mtpDeviceStructPointer, 0);
        }

        /// <summary>
        /// This function retrieves the contents of a certain folder with id parent on a certain storage
        /// on a certain device. The result contains both files and folders.
        /// The device used with this operations must have been opened with
        /// LIBMTP_Open_Raw_Device_Uncached() or it will fail.
        /// NOTE: the request will always perform I/O with the device.
        /// </summary>
        /// <param name="mtpDeviceStructPointer">device a pointer to the MTP device to report info from.</param>
        /// <param name="storageId">storage a storage on the device to report info from.
        /// If 0 is passed in, the files for the given parent will be searched across all available storages.</param>
        /// <param name="parentId">the parent folder id.</param>
        /// <returns></returns>
        [DllImport(LibMtpName)]
        private static extern IntPtr LIBMTP_Get_Files_And_Folders(IntPtr mtpDeviceStructPointer, uint storageId, uint parentId);
        
        internal const uint LibmtpFilesAndFoldersRoot = 0xffffffff;
        
        public static IntPtr GetParentContent(IntPtr mtpDeviceStructPointer, uint storageId, uint parentId)
        {
            return LIBMTP_Get_Files_And_Folders(mtpDeviceStructPointer, storageId, parentId);
        }

        /// <summary>
        /// This create a folder on the current MTP device. The PTP name for a folder is "association".
        /// The PTP/MTP devices does not have an internal "folder" concept really, it contains a flat list of all files
        /// and some file are "associations" that other files and folders may refer to as its "parent"
        /// </summary>
        /// <param name="mtpDeviceStructPointer">a pointer to the device to create the folder on.</param>
        /// <param name="name">the name of the new folder.
        /// Note this can be modified if the device does not support all the characters in the name.</param>
        /// <param name="parentId">id of parent folder to add the new folder to,
        /// or 0xFFFFFFFF to put it in the root directory.</param>
        /// <param name="storageId">id of the storage to add this new folder to.
        /// notice that you cannot mismatch storage id and parent id: they must both be on the same storage!
        /// Pass in 0 if you want to create this folder on the default storage.</param>
        /// <returns>id to new folder or 0 if an error occurred</returns>
        [DllImport(LibMtpName)]
        private static extern uint LIBMTP_Create_Folder(IntPtr mtpDeviceStructPointer, 
            [MarshalAs(UnmanagedType.LPUTF8Str)] string name, uint parentId, uint storageId);

        public static uint CreateFolder(IntPtr mtpDeviceStructPointer, string name, uint parentId, uint storageId)
        {
            return LIBMTP_Create_Folder(mtpDeviceStructPointer, name, parentId, storageId);
        }

        [DllImport(LibMtpName)]
        private static extern int LIBMTP_Send_File_From_File(IntPtr device, [MarshalAs(UnmanagedType.LPUTF8Str)] string path,
            ref FileStruct filedata, ProgressFunction? callback, IntPtr data);

        public static int SendFile(IntPtr device, string path, ref FileStruct fileData, ProgressFunction? callback,
            IntPtr data)
        {
            return LIBMTP_Send_File_From_File(device, path, ref fileData, callback, data);
        }
        
        /// <summary>
        /// This destroys a file metadata structure and deallocates the memory used by it, including any strings.
        /// Never use a file metadata structure again after calling this function on it.
        /// </summary>
        /// <param name="file">file the file metadata to destroy.</param>
        [DllImport(LibMtpName)]
        private static extern void LIBMTP_destroy_file_t(IntPtr file);

        public static void DestroyFile(IntPtr fileStructPointer)
        {
            LIBMTP_destroy_file_t(fileStructPointer);
        }

        /// <summary>
        /// This function deletes a single file, track, playlist, folder or any other object off the MTP device,
        /// identified by the object ID. If you delete a folder, there is no guarantee that the device will really
        /// delete all the files that were in that folder, rather it is expected that they will not be deleted,
        /// and will turn up in object listings with parent set to a non-existant object ID. The safe way to do this
        /// is to recursively delete all files (and folders) contained in the folder, then the folder itself.
        /// </summary>
        /// <param name="device">a pointer to the device to delete the object from</param>
        /// <param name="objectId">the object to delete</param>
        /// <returns>0 on success, any other value means failure.</returns>
        [DllImport(LibMtpName)]
        private static extern int LIBMTP_Delete_Object(IntPtr device, uint objectId);
        
        public static int DeleteObject(IntPtr device, uint objectId)
        {
            return LIBMTP_Delete_Object(device, objectId);
        }
        
        /// <summary>
        /// This returns a list of all folders available on the current MTP device.
        /// </summary>
        /// <param name="device">a pointer to the device to get the folder listing for.</param>
        /// <param name="storage">a storage ID to get the folder list from</param>
        /// <returns>a list of folders</returns>
        [DllImport(LibMtpName)]
        private static extern IntPtr LIBMTP_Get_Folder_List_For_Storage(IntPtr device, uint storage);
        
        public static IntPtr GetFolderListForStorage(IntPtr device, uint storageId)
        {
            return LIBMTP_Get_Folder_List_For_Storage(device, storageId);
        }
        
        /// <summary>
        /// This returns a list of all folders available on the current MTP device.
        /// </summary>
        /// <param name="device">a pointer to the device to get the folder listing for.</param>
        /// <returns>a list of folders</returns>
        [DllImport(LibMtpName)]
        private static extern IntPtr LIBMTP_Get_Folder_List(IntPtr device);
        
        public static IntPtr GetFolderList(IntPtr device)
        {
            return LIBMTP_Get_Folder_List(device);
        }

        /// <summary>
        /// This recursively deletes the memory for a folder structure.
        /// This shall typically be called on a top-level folder list to detsroy the entire folder tree.
        /// </summary>
        /// <param name="folder">folder structure to destroy</param>
        [DllImport(LibMtpName)]
        private static extern void LIBMTP_destroy_folder_t(IntPtr folder);
        
        public static void DestroyFolder(IntPtr folder)
        {
            LIBMTP_destroy_folder_t(folder);
        }

        /// <summary>
        /// This returns a long list of all files available on the current MTP device.
        /// Folders will not be returned, but abstract entities like playlists and albums will show up as "files".
        /// If you want to group your file listing by storage (per storage unit) or arrange files into folders,
        /// you must dereference the <code>storage_id</code> and/or <code>parent_id</code> field of the returned
        /// <code>LIBMTP_file_t</code> struct. To arrange by folders or files you typically have to create the proper
        /// trees by calls to <code>LIBMTP_Get_Storage()</code> and/or <code>LIBMTP_Get_Folder_List()</code> first.
        /// </summary>
        /// <param name="device">a pointer to the device to get the file listing for.</param>
        /// <param name="callback">a function to be called during the tracklisting retrieveal for displaying progress
        /// bars etc, or NULL if you don't want any callbacks.</param>
        /// <param name="data">a user-defined pointer that is passed along to the <code>progress</code> function
        /// in order to pass along some user defined data to the progress updates. If not used, set this to NULL.</param>
        /// <returns>a list of files that can be followed using the <code>next</code> field of the
        /// <code>LIBMTP_file_t</code> data structure. Each of the metadata tags must be freed after use, and may
        /// contain only partial metadata information, i.e. one or several fields may be NULL or 0.</returns>
        [DllImport(LibMtpName)]
        private static extern IntPtr LIBMTP_Get_Filelisting_With_Callback(IntPtr device, ProgressFunction? callback,
            IntPtr data);

        public static IntPtr GetFilelistingWithCallback(IntPtr device, ProgressFunction? callback)
        {
            return LIBMTP_Get_Filelisting_With_Callback(device, callback, IntPtr.Zero);
        }

        /// <summary>
        /// This gets a file off the device to a local file identified by a filename.
        /// </summary>
        /// <param name="device">a pointer to the device to get the track from.</param>
        /// <param name="id">the file ID of the file to retrieve.</param>
        /// <param name="path">a filename to use for the retrieved file.</param>
        /// <param name="progressCallback">a progress indicator function or NULL to ignore.</param>
        /// <param name="data">a user-defined pointer that is passed along to the <code>progress</code> function
        /// in order to pass along some user defined data to the progress updates. If not used, set this to NULL.</param>
        /// <returns>0 if the transfer was successful, any other value means failure.</returns>
        [DllImport(LibMtpName)]
        private static extern int LIBMTP_Get_File_To_File(IntPtr device, uint id,
            [MarshalAs(UnmanagedType.LPUTF8Str)] string path, ProgressFunction? progressCallback, IntPtr data);
        
        public static int GetFileToFile(IntPtr device, uint id, string filePath, ProgressFunction? progressCallback)
        {
            return LIBMTP_Get_File_To_File(device, id, filePath, progressCallback, IntPtr.Zero);
        }
    }
}