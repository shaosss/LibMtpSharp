using System;
using System.Runtime.InteropServices;

namespace LibMtpSharp.Structs;

public struct AlbumStruct
{
    public uint AlbumId { get; set; }
    public uint ParentId { get; set; }
    public uint StorageId { get; set; }
    public string Name { get; set; }
    public string Artist { get; set; }
    public string Composer { get; set; }
    public string Genre { get; set; }
    public uint[] Tracks { get; set; }

    internal AlbumStruct(AlbumNativeStruct albumNativeStruct)
    {
        AlbumId = albumNativeStruct.album_id;
        ParentId = albumNativeStruct.parent_id;
        StorageId = albumNativeStruct.storage_id;
        Name = albumNativeStruct.name;
        Artist = albumNativeStruct.artist;
        Composer = albumNativeStruct.composer;
        Genre = albumNativeStruct.genre;
        Tracks = new uint[albumNativeStruct.no_tracks];
        for (int i = 0; i < albumNativeStruct.no_tracks; i++)
        {
            Tracks[i] = (uint)Marshal.ReadInt64(albumNativeStruct.tracks, i*sizeof(uint));
        }
    }

    internal void SendAlbum(IntPtr mptDeviceStructPointer, bool createNew)
    {
        GCHandle tracksHandle = GCHandle.Alloc(Tracks, GCHandleType.Pinned);
        try
        {
            IntPtr tracksPtr = tracksHandle.AddrOfPinnedObject();
            var albumNativeStruct = new AlbumNativeStruct()
            {
                album_id = AlbumId,
                parent_id = ParentId,
                storage_id = StorageId,
                name = Name,
                artist = Artist,
                composer = Composer,
                genre = Genre,
                no_tracks = (uint)Tracks.Length,
                tracks = tracksPtr
            };
            if (createNew)
                NativeAPI.LibMtpLibrary.CreateNewAlbum(mptDeviceStructPointer, ref albumNativeStruct);
            else
                NativeAPI.LibMtpLibrary.UpdateAlbum(mptDeviceStructPointer, ref albumNativeStruct);
        }
        finally
        {
            tracksHandle.Free();
        }
    }
}