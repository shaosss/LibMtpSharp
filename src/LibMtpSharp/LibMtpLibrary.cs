using System;
using System.Runtime.InteropServices;
using LibMtpSharp.Enums;

namespace LibMtpSharp
{
    /// <summary>
    /// LibMtp API.
    /// </summary>
    internal partial class LibMtpLibrary
    {
        private const string LibMtpName = "libmtp";

        [DllImport(LibMtpName)]
        private static extern void LIBMTP_Init();
        [DllImport(LibMtpName)]
        private static extern void LIBMTP_Init_MTPZ([MarshalAs(UnmanagedType.LPUTF8Str)] string publicExponent, 
            [MarshalAs(UnmanagedType.LPUTF8Str)] string hexenckey, [MarshalAs(UnmanagedType.LPUTF8Str)] string modulus, 
            [MarshalAs(UnmanagedType.LPUTF8Str)] string privateKey, [MarshalAs(UnmanagedType.LPUTF8Str)] string hexcerts);

        static LibMtpLibrary()
        {
            LIBMTP_Init();
            LIBMTP_Init_MTPZ(MTPZ.publicExponent, MTPZ.encryptionKeyHex, MTPZ.modulus, MTPZ.privateKey, MTPZ.certificateHex);
        }
        
        [DllImport(LibMtpName)]
        private static extern void LIBMTP_Free(IntPtr ptrToFree);
        
        public static void Free(IntPtr ptrToFree)
        {
            LIBMTP_Free(ptrToFree);
        }
        
        [DllImport(LibMtpName)]
        private static extern void LIBMTP_Set_Debug(int level);

        public static void SetDebug(DebugLevelEnum level)
        {
            LIBMTP_Set_Debug((int)level);
        }
    }
}