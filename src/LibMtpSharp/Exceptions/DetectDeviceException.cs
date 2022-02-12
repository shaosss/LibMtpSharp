using System;
using LibMtpSharp.Enums;

namespace LibMtpSharp.Exceptions;

public class DetectDeviceException : ApplicationException
{
    public DetectDeviceException(ErrorEnum error)
        : base("")
    {
    }
}