using System.Runtime.InteropServices;

namespace LibMtpSharp.Structs
{
    /// <summary>
    /// A data structure to hold MTP device entries.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal struct DeviceEntry
    {
        /// <summary>
        /// The vendor of this device
        /// </summary>
        [MarshalAs(UnmanagedType.LPUTF8Str)]
        public string vendor;
        
        /// <summary>
        ///  Vendor ID for this device
        /// </summary>
        public ushort vendor_id;
        
        /// <summary>
        /// The product name of this device
        /// </summary>
        [MarshalAs(UnmanagedType.LPUTF8Str)]
        public string product;
        
        /// <summary>
        /// Product ID for this device
        /// </summary>
        public ushort product_id;
        
        /// <summary>
        /// Bugs, device specifics etc
        /// </summary>
        public uint device_flags;
        
        /// <summary>
        /// Product ID for this device
        /// </summary>
        public ushort device_bcd;

        public override string ToString()
        {
            return $"(VID={vendor_id:X}, PID={product_id:X}, REV={device_bcd:X})";
        }
    }
}