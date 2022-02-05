using System;

namespace LibMtpSharp.Exceptions
{
    public class PopulateStoragesException : ApplicationException
    {
        public OpenedMtpDevice Device { get; }

        public PopulateStoragesException(OpenedMtpDevice device)
            :base($"Failed to populate storages for {device}")
        {
            Device = device;
        }
    }
}