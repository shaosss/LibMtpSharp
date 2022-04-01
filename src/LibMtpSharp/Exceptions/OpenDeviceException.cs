using System;
using LibMtpSharp.Structs;

namespace LibMtpSharp.Exceptions
{
    public class OpenDeviceException : ApplicationException
    {
        public OpenDeviceException(RawDevice rawDevice)
            :base($"Failed to open {rawDevice}") { }
    }
}