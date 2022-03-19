using System.Runtime.InteropServices;

namespace LibMtpSharp.Structs
{
    /// <summary>
    /// A data structure to hold MTP device entries.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct DeviceEntry
    {
        /// <summary>
        /// The vendor of this device
        /// </summary>
        [MarshalAs(UnmanagedType.LPUTF8Str)]
        public string Vendor;
        
        /// <summary>
        ///  Vendor ID for this device
        /// </summary>
        public ushort VendorId;
        
        /// <summary>
        /// The product name of this device
        /// </summary>
        [MarshalAs(UnmanagedType.LPUTF8Str)]
        public string Product;
        
        /// <summary>
        /// Product ID for this device
        /// </summary>
        public ushort ProductId;
        
        /// <summary>
        /// Bugs, device specifics etc
        /// </summary>
        public uint DeviceFlags;
        
        /// <summary>
        /// Product ID for this device
        /// </summary>
        public ushort DeviceBcd;

        public override string ToString()
        {
            return $"(VID={VendorId:X}, PID={ProductId:X}, REV={DeviceBcd:X})";
        }
    }
}