using System;
using System.Runtime.InteropServices;
using LibMtpSharp.Structs;

namespace LibMtpSharp.NativeAPI
{
    /// <summary>
    /// The callback type definition. Notice that a progress percentage ratio is easy to calculate by dividing
    /// <code>sent</code> by <code>total</code>.
    /// @param sent the number of bytes sent so far
    /// @param total the total number of bytes to send
    /// @param data a user-defined dereferencable pointer
    /// @return if anything else than 0 is returned, the current transfer will be interrupted / cancelled.
    /// </summary>
    public delegate int ProgressFunction(ulong sent, ulong total, IntPtr data);

    /// <summary>
    /// Callback function for get by handler function
    /// params the device parameters
    /// @param priv a user-defined dereferencable pointer
    /// @param wantlen the number of bytes wanted
    /// @param data a buffer to write the data to
    /// @param gotlen pointer to the number of bytes actually written to data
    /// @return LIBMTP_HANDLER_RETURN_OK if successful,
    /// LIBMTP_HANDLER_RETURN_ERROR on error or LIBMTP_HANDLER_RETURN_CANCEL to cancel the transfer
    /// </summary>
    public delegate ushort MtpDataGetFunction(IntPtr parameters, IntPtr priv,
        uint wantlen, IntPtr data, out uint gotlen);
        
    public enum HandlerReturn : ushort
    { 
        Ok = 0,
        Error = 1,
        Cancel = 2
    }
    
    /// <summary>
    /// Media Items API.
    /// </summary>
    internal partial class LibMtpLibrary
    {
        /// <summary>
        /// This function returns a list of the albums available on the device.
        /// </summary>
        /// <param name="device">device a pointer to the device to get the album listing from.</param>
        /// <returns>an album list on success, else NULL. If there are no albums on the device,
        /// NULL will be returned as well.</returns>
        [DllImport(LibMtpName)]
        private static extern IntPtr LIBMTP_Get_Album_List(IntPtr device);

        public static IntPtr GetAlbums(IntPtr mtpDeviceStructPointer)
        {
            return LIBMTP_Get_Album_List(mtpDeviceStructPointer);
        }
        
        /// <summary>
        /// This recursively deletes the memory for an album structure
        /// </summary>
        /// <param name="album">album structure to destroy</param>
        [DllImport(LibMtpName)]
        private static extern void LIBMTP_destroy_album_t(IntPtr album);

        public static void FreeAlbum(IntPtr albumsStructPtr)
        {
            LIBMTP_destroy_album_t(albumsStructPtr);
        }
        
        /// <summary>
        /// This returns a long list of all tracks available on the current MTP device.
        /// Tracks include multimedia objects, both music tracks and video tracks.
        /// </summary>
        /// <param name="mtpDeviceStructPointer"> a pointer to the device to get the track listing for.</param>
        /// <param name="callback">a function to be called during the tracklisting retrieval for displaying
        /// progress bars etc, or NULL if you don't want any callbacks.</param>
        /// <param name="data">a user-defined pointer that is passed along to the <code>progress</code> function
        /// in order to pass along some user defined data to the progress updates. If not used, set this to NULL.</param>
        /// <returns>a list of tracks that can be followed using the <code>next</code>
        /// field of the <code>LIBMTP_track_t</code> data structure. Each of the metadata tags must be freed after use,
        /// and may contain only partial metadata information, i.e. one or several fields may be NULL or 0.</returns>
        [DllImport(LibMtpName)]
        private static extern IntPtr LIBMTP_Get_Tracklisting_With_Callback(IntPtr mtpDeviceStructPointer, ProgressFunction callback, IntPtr data);
        
        public static IntPtr GetTracks(IntPtr mtpDeviceStructPointer)
        {
            return LIBMTP_Get_Tracklisting_With_Callback(mtpDeviceStructPointer, null, IntPtr.Zero);
        }

        /// <summary>
        /// This function sends a track from a handler function to an MTP device.
        /// A filename and a set of metadata must be given as input.
        /// </summary>
        /// <param name="device">a pointer to the device to send the track to.</param>
        /// <param name="getFunc">the function to call when we have data.</param>
        /// <param name="priv">the user-defined pointer that is passed to <code>get_func</code>.</param>
        /// <param name="metadata"> a track metadata set to be written along with the file.
        /// After this call the field <code>metadata->item_id</code> will contain the new track ID. Other fields such
        /// as the <code>metadata->filename</code>, <code>metadata->parent_id</code> or <code>metadata->storage_id</code>
        /// may also change during this operation due to device restrictions, so do not rely on the contents of this
        /// struct to be preserved in any way.
        /// <ul>
        /// <li><code>metadata-&gt;parent_id</code> should be set to the parent (e.g. folder) to store this track in.
        /// Since some devices are a bit picky about where files are placed, a default folder will be chosen if libmtp
        /// has detected one for the current filetype and this parameter is set to 0. If this is 0 and no default folder
        /// can be found, the file will be stored in the root folder.</li>
        /// <li><code>metadata-&gt;storage_id</code> should be set to the desired storage (e.g. memory card or whatever
        /// your device presents) to store this track in. Setting this to 0 will store the track on the primary storage.</li>
        /// </ul></param>
        /// <param name="callback">a progress indicator function or NULL to ignore.</param>
        /// <param name="data">a user-defined pointer that is passed along to the <code>progress</code> function in order
        /// to pass along some user defined data to the progress updates. If not used, set this to NULL.</param>
        /// <returns>0 if the transfer was successful, any other value means failure.</returns>
        [DllImport(LibMtpName)]
        private static extern int LIBMTP_Send_Track_From_Handler(IntPtr device, MtpDataGetFunction getFunc, IntPtr priv,
            ref TrackStruct metadata, ProgressFunction callback, IntPtr data);

        public static int SendTrackFromHandler(IntPtr device, MtpDataGetFunction getFunction, ref TrackStruct trackStruct, 
            ProgressFunction progressFunction)
        {
            return LIBMTP_Send_Track_From_Handler(device, getFunction, IntPtr.Zero, ref trackStruct, progressFunction,
                IntPtr.Zero);
        }

        /// <summary>
        /// This destroys a track metadata structure and deallocates the memory used by it, including any strings.
        /// Never use a track metadata structure again after calling this function on it.
        /// </summary>
        /// <param name="track"> the track metadata to destroy.</param>
        [DllImport(LibMtpName)]
        private static extern void LIBMTP_destroy_track_t(IntPtr track);

        public static void FreeTrack(IntPtr trackStructPtr)
        {
            LIBMTP_destroy_track_t(trackStructPtr);
        }
        
        /// <summary>
        /// This routine creates a new album based on the metadata supplied.
        /// If the <code>tracks</code> field of the metadata contains a track listing,
        /// these tracks will be added to the album.
        /// </summary>
        /// <param name="device">a pointer to the device to create the new album on</param>
        /// <param name="metadata">the metadata for the new album. If the function exits with success,
        /// the <code>album_id</code> field of this struct will contain the new ID of the album.
        /// <ul>
        ///     <li><code>metadata-&gt;parent_id</code> should be set to the parent (e.g. folder) to store this track in.
        ///     Since some devices are a bit picky about where files are placed, a default folder will be chosen
        ///     if libmtp has detected one for the current filetype and this parameter is set to 0.
        ///     If this is 0 and no default folder can be found, the file will be stored in the root folder.
        ///     </li>
        ///     <li><code>metadata-&gt;storage_id</code> should be set to the desired storage
        ///     (e.g. memory card or whatever your device presents) to store this track in.
        ///     Setting this to 0 will store the track on the primary storage.
        ///     </li>
        /// </ul></param>
        /// <returns>0 on success, any other value means failure.</returns>
        [DllImport(LibMtpName)]
        private static extern int LIBMTP_Create_New_Album(IntPtr device, ref AlbumNativeStruct metadata);

        public static int CreateNewAlbum(IntPtr device, ref AlbumNativeStruct albumNativeStruct)
        {
            return LIBMTP_Create_New_Album(device, ref albumNativeStruct);
        }
        
        /// <summary>
        /// This routine updates an album based on the metadata supplied.
        /// If the <code>tracks</code> field of the metadata contains a track listing, these tracks will be added to the
        /// album in place of those already present, i.e. the previous track listing will be deleted.
        /// </summary>
        /// <param name="device">a pointer to the device to create the new album on</param>
        /// <param name="metadata">the metadata for the album to be updated
        ///     !notice that the field <code>album_id</code> must contain the appropriate album ID.</param>
        /// <returns>0 on success, any other value means failure.</returns>
        [DllImport(LibMtpName)]
        private static extern int LIBMTP_Update_Album(IntPtr device, ref AlbumNativeStruct metadata);
        
        public static int UpdateAlbum(IntPtr device, ref AlbumNativeStruct albumNativeStruct)
        {
            return LIBMTP_Update_Album(device, ref albumNativeStruct);
        }

        /// <summary>
        /// This creates a new sample data metadata structure and allocates memory for it.
        /// Notice that if you add strings to this structure they will be freed by the corresponding
        /// <code>LIBMTP_destroy_sampledata_t</code> operation later, so be careful of using strdup() when assigning
        /// strings.
        /// </summary>
        /// <returns>a pointer to the newly allocated metadata structure.</returns>
        [DllImport(LibMtpName)]
        private static extern IntPtr LIBMTP_new_filesampledata_t();
        
        public static IntPtr CreateFileSampleData()
        {
            return LIBMTP_new_filesampledata_t();
        }
        
        /// <summary>
        /// This destroys a file sample metadata type.
        /// </summary>
        /// <param name="fileSapmpleData">the file sample metadata to be destroyed.</param>
        [DllImport(LibMtpName)]
        private static extern void LIBMTP_destroy_filesampledata_t(IntPtr fileSapmpleData);
        
        public static void FreeFileSampleData(IntPtr fileSampleDataPtr)
        {
            LIBMTP_destroy_filesampledata_t(fileSampleDataPtr);
        }
        
        /// <summary>
        /// This routine gets representative sample data for an object.
        /// This uses the RepresentativeSampleData property of the album, if the device supports it.
        /// </summary>
        /// <param name="libMtpDevice">a pointer to the device which the object is on.</param>
        /// <param name="objectId">unique id of the object to get data for.</param>
        /// <param name="fileSampleData">pointer to LIBMTP_filesampledata_t struct to receive data</param>
        /// <returns>0 on success, any other value means failure.</returns>
        [DllImport(LibMtpName)]
        private static extern int LIBMTP_Get_Representative_Sample(IntPtr libMtpDevice, uint objectId, IntPtr fileSampleData);
        
        /// <summary>
        /// Gets representative sample data. For some reason not working with Zune album covers
        /// </summary>
        /// <param name="libMtpDevice"></param>
        /// <param name="objectId"></param>
        /// <param name="fileSampleData"></param>
        /// <returns></returns>
        public static int GetRepresentativeSample(IntPtr libMtpDevice, uint objectId, IntPtr fileSampleData)
        {
            return LIBMTP_Get_Representative_Sample(libMtpDevice, objectId, fileSampleData);
        }

        [DllImport(LibMtpName)]
        private static extern int LIBMTP_Send_Representative_Sample(IntPtr device, uint id, ref FileSampleDataNativeStruct sampledata);

        public static int SendRepresentativeSample(IntPtr device, uint id, ref FileSampleDataNativeStruct sampledata)
        {
            return LIBMTP_Send_Representative_Sample(device, id, ref sampledata);
        }
    }
}