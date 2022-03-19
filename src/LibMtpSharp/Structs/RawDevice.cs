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
        public DeviceEntry DeviceEntry;
        
        /// <summary>
        /// Location of the bus, if device available
        /// </summary>
        internal uint BusLocation;
        
        /// <summary>
        /// Device number on the bus, if device available
        /// </summary>
        internal byte DevNum;

        public override string ToString()
        {
            return $"Device {DeviceEntry} located at bus: {BusLocation}, devNum: {DevNum}";
        }
    }
}