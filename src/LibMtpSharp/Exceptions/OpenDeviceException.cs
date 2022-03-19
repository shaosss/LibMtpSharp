using System;
using LibMtpSharp.Structs;

namespace LibMtpSharp.Exceptions
{
    internal class OpenDeviceException : ApplicationException
    {
        public OpenDeviceException(RawDevice rawDevice)
            :base($"Failed to open {rawDevice}") { }
    }
}