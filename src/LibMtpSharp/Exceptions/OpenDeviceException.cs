using System;
using LibMtpSharp.Structs;

namespace LibMtpSharp.Exceptions
{
    internal class OpenDeviceException : ApplicationException
    {
        public RawDevice RawDevice { get; }
        
        public OpenDeviceException(RawDevice rawDevice)
            :base($"Failed to open {rawDevice}")
        {
            RawDevice = rawDevice;
        }
    }
}