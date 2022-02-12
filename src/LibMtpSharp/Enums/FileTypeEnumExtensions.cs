namespace LibMtpSharp.Enums
{
    public static class FileTypeEnumExtensions
    {
        /// <summary>
        /// Audio filetype test.
        /// For filetypes that can be either audio or video, use <see cref="IsAudioOrVideo"/>
        /// </summary>
        /// <param name="fileType"></param>
        /// <returns></returns>
        public static bool IsAudio(this FileTypeEnum fileType)
        {
            return fileType == FileTypeEnum.Wav ||
                   fileType == FileTypeEnum.Mp3 ||
                   fileType == FileTypeEnum.Mp2 ||
                   fileType == FileTypeEnum.Wma ||
                   fileType == FileTypeEnum.Ogg ||
                   fileType == FileTypeEnum.Flac ||
                   fileType == FileTypeEnum.Aac ||
                   fileType == FileTypeEnum.M4A ||
                   fileType == FileTypeEnum.Audible ||
                   fileType == FileTypeEnum.UndefinedAudio;
        }
        
        /// <summary>
        /// Video filetype test.
        /// For filetypes that can be either audio or video, use <see cref="IsAudioOrVideo"/>
        /// </summary>
        /// <param name="fileType"></param>
        /// <returns></returns>
        public static bool IsVideo(this FileTypeEnum fileType)
        {
            return fileType == FileTypeEnum.Wmv ||
                   fileType == FileTypeEnum.Avi ||
                   fileType == FileTypeEnum.Mpeg ||
                   fileType == FileTypeEnum.UndefinedVideo;
        }
        
        /// <summary>
        /// Audio and/or video filetype test.
        /// </summary>
        /// <param name="fileType"></param>
        /// <returns></returns>
        public static bool IsAudioOrVideo(this FileTypeEnum fileType)
        {
            return fileType == FileTypeEnum.Mp4 ||
                   fileType == FileTypeEnum.Asf ||
                   fileType == FileTypeEnum.Qt;
        }

        /// <summary>
        /// Test if filetype is a track.
        /// Use this to determine if the File API or Track API should be used to upload or download an object.
        /// </summary>
        /// <param name="fileType"></param>
        /// <returns></returns>
        public static bool IsTrack(this FileTypeEnum fileType)
        {
            return IsAudio(fileType) || IsVideo(fileType) || IsAudioOrVideo(fileType);
        }
        
        /// <summary>
        /// Image filetype test
        /// </summary>
        /// <param name="fileType"></param>
        /// <returns></returns>
        public static bool IsImage(this FileTypeEnum fileType)
        {
            return fileType == FileTypeEnum.Jpeg ||
                   fileType == FileTypeEnum.Jfif ||
                   fileType == FileTypeEnum.Tiff ||
                   fileType == FileTypeEnum.Bmp ||
                   fileType == FileTypeEnum.Gif ||
                   fileType == FileTypeEnum.Pict ||
                   fileType == FileTypeEnum.Png ||
                   fileType == FileTypeEnum.Jp2 ||
                   fileType == FileTypeEnum.Jpx ||
                   fileType == FileTypeEnum.WindowsImageFormat;
        }
        
        /// <summary>
        /// Address book and Business card filetype test
        /// </summary>
        /// <param name="fileType"></param>
        /// <returns></returns>
        public static bool IsAddressBook(this FileTypeEnum fileType)
        {
            return fileType == FileTypeEnum.VCard2 ||
                   fileType == FileTypeEnum.VCard3;
        }
        
        /// <summary>
        /// Audio and/or video filetype test.
        /// </summary>
        /// <param name="fileType"></param>
        /// <returns></returns>
        public static bool IsCalendar(this FileTypeEnum fileType)
        {
            return fileType == FileTypeEnum.VCalendar1 ||
                   fileType == FileTypeEnum.VCalendar2;
        }
    }
}