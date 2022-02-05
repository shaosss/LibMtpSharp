using System.Runtime.InteropServices;

namespace LibMtpSharp.Structs
{
    /// <summary>
    /// A data structure to hold a raw MTP device connected to the bus.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct RawDevice
    {
        /// <summary>
        /// The device entry for this raw device
        /// </summary>
        internal DeviceEntry device_entry;
        
        /// <summary>
        /// Location of the bus, if device available
        /// </summary>
        internal uint bus_location;
        
        /// <summary>
        /// Device number on the bus, if device available
        /// </summary>
        internal byte devnum;

        public override string ToString()
        {
            return $"Device {device_entry} located at bus: {bus_location}, devNum: {devnum}";
        }
    }
}