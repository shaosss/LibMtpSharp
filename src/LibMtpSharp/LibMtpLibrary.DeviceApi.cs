using System;
using System.Runtime.InteropServices;
using LibMtpSharp.Enums;
using LibMtpSharp.Structs;

namespace LibMtpSharp
{
    /// <summary>
    /// The basic device management API.
    /// </summary>
    internal partial class LibMtpLibrary
    {
        /// <summary>
        /// Detect the raw MTP device descriptors and return a list of the devices found.
        /// </summary>
        /// <param name="listOfDevices">a pointer to a the list of raw devices found.
        /// This may be NULL on return if the number of detected devices is zero.
        /// The user shall call <see cref="LIBMTP_Free"/> this variable when finished with the raw devices,
        /// in order to release memory.</param>
        /// <param name="numberOfDevices">the number of devices in the list. This may be 0.</param>
        /// <returns></returns>
        [DllImport(LibMtpName)]
        private static extern ErrorEnum LIBMTP_Detect_Raw_Devices(ref IntPtr listOfDevices, ref int numberOfDevices);

        public static ErrorEnum DetectRawDevices(ref IntPtr listOfDevices, ref int numberOfDevices)
        {
            return LIBMTP_Detect_Raw_Devices(ref listOfDevices, ref numberOfDevices);
        }
        
        /// <summary>
        /// This function opens a device from a raw device. It is the preferred way to access devices in
        /// the new interface where several devices can come and go as the library is working on a certain device.
        /// </summary>
        /// <param name="rawDevice">the raw device to open a "real" device for.</param>
        /// <returns>an open device</returns>
        [DllImport(LibMtpName)]
        private static extern IntPtr LIBMTP_Open_Raw_Device( ref RawDevice rawDevice);
        
        [DllImport(LibMtpName)]
        private static extern IntPtr LIBMTP_Open_Raw_Device_Uncached( ref RawDevice rawDevice);
        
        public static IntPtr OpenRawDevice(ref RawDevice rawDevice)
        {
            return LIBMTP_Open_Raw_Device(ref rawDevice);
        }
        
        public static IntPtr OpenRawDeviceUncached(ref RawDevice rawDevice)
        {
            return LIBMTP_Open_Raw_Device_Uncached(ref rawDevice);
        }
        
        /// <summary>
        /// This closes and releases an allocated MTP device.
        /// </summary>
        /// <param name="mtpDeviceStructPointer">a pointer to the MTP device to release.</param>
        [DllImport(LibMtpName)]
        private static extern void LIBMTP_Release_Device(IntPtr mtpDeviceStructPointer);
        
        public static void ReleaseDevice(IntPtr mtpDevice)
        {
            LIBMTP_Release_Device(mtpDevice);
        }
        
        /// <summary>
        /// This retrieves the manufacturer name of an MTP device.
        /// </summary>
        /// <param name="mtpDeviceStructPointer">a pointer to the device to get the manufacturer name for.</param>
        /// <returns>a newly allocated UTF-8 string representing the manufacturer name.
        /// The string must be freed by the caller after use. If the call was unsuccessful this will contain NULL.
        /// </returns>
        [DllImport(LibMtpName)]
        private static extern IntPtr LIBMTP_Get_Manufacturername(IntPtr mtpDeviceStructPointer);
        
        public static string GetManufacturerName(IntPtr openedMtpDevice)
        {
            var pointerToStr = LIBMTP_Get_Manufacturername(openedMtpDevice);
            var manufacturerName = Marshal.PtrToStringUTF8(pointerToStr);
            if (manufacturerName != null)
                LIBMTP_Free(pointerToStr);
            return manufacturerName;
        }
        
        /// <summary>
        /// This retrieves the model name (often equal to product name) of an MTP device.
        /// </summary>
        /// <param name="mtpDeviceStructPointer">a pointer to the device to get the model name for.</param>
        /// <returns>a newly allocated UTF-8 string representing the model name.
        /// The string must be freed by the caller after use. If the call was unsuccessful this will contain NULL.
        /// </returns>
        [DllImport(LibMtpName)]
        private static extern IntPtr LIBMTP_Get_Modelname(IntPtr mtpDeviceStructPointer);
        
        public static string GetModelName(IntPtr mtpDeviceStructPointer)
        {
            var pointerToStr = LIBMTP_Get_Modelname(mtpDeviceStructPointer);
            var modelName = Marshal.PtrToStringUTF8(pointerToStr);
            if (modelName != null)
                LIBMTP_Free(pointerToStr);
            return modelName;
        }
        
        /// <summary>
        /// This retrieves the serial number of an MTP device.
        /// </summary>
        /// <param name="mtpDeviceStructPointer">a pointer to the device to get the serial number for.</param>
        /// <returns>a newly allocated UTF-8 string representing the serial number.
        /// The string must be freed by the caller after use. If the call was unsuccessful this will contain NULL.
        /// </returns>
        [DllImport(LibMtpName)]
        private static extern IntPtr LIBMTP_Get_Serialnumber(IntPtr mtpDeviceStructPointer);

        public static string GetSerialNumber(IntPtr mtpDeviceStructPointer)
        {
            var pointerToStr = LIBMTP_Get_Serialnumber(mtpDeviceStructPointer);
            var serialNumber = Marshal.PtrToStringUTF8(pointerToStr);
            if (serialNumber != null)
                LIBMTP_Free(pointerToStr);
            return serialNumber;
        }
        
        /// <summary>
        /// This retrieves the device version (hardware and firmware version) of an MTP device.
        /// </summary>
        /// <param name="mtpDeviceStructPointer">a pointer to the device to get the device version for.</param>
        /// <returns>a newly allocated UTF-8 string representing the device version.
        /// The string must be freed by the caller after use. If the call was unsuccessful this will contain NULL.
        /// </returns>
        [DllImport(LibMtpName)]
        private static extern IntPtr LIBMTP_Get_Deviceversion(IntPtr mtpDeviceStructPointer);

        public static string GetDeviceVersion(IntPtr mtpDeviceStructPointer)
        {
            var pointerToStr = LIBMTP_Get_Deviceversion(mtpDeviceStructPointer);
            var deviceVersion = Marshal.PtrToStringUTF8(pointerToStr);
            if (deviceVersion != null)
                LIBMTP_Free(pointerToStr);
            return deviceVersion;
        }
        
        /// <summary>
        /// This retrieves the "friendly name" of an MTP device. Usually this is simply the name of the owner
        /// or something like "John Doe's Digital Audio Player". This property should be supported by all MTP devices.
        /// </summary>
        /// <param name="mtpDeviceStructPointer">a pointer to the device to get the friendly name for.</param>
        /// <returns>a newly allocated UTF-8 string representing the friendly name.
        /// The string must be freed by the caller after use.</returns>
        [DllImport(LibMtpName)]
        private static extern IntPtr LIBMTP_Get_Friendlyname(IntPtr mtpDeviceStructPointer);

        public static string GetFriendlyName(IntPtr mtpDeviceStructPointer)
        {
            var pointerToStr = LIBMTP_Get_Friendlyname(mtpDeviceStructPointer);
            var friendlyName = Marshal.PtrToStringUTF8(pointerToStr);
            if (friendlyName != null)
                LIBMTP_Free(pointerToStr);
            return friendlyName;
        }
        
        /// <summary>
        ///  This function retrieves the current battery level on the device.
        /// </summary>
        /// <param name="mtpDeviceStructPointer">a pointer to the device to get the battery level for.</param>
        /// <param name="maxLevel">a pointer to a variable that will hold the maximum level of the battery
        /// if the call was successful.</param>
        /// /// <param name="currentLevel">a pointer to a variable that will hold the current level of the battery
        /// if the call was successful. A value of 0 means that the device is on external power.</param>
        /// <returns>0 if the storage info was successfully retrieved, any other means failure.
        /// A typical cause of failure is that the device does not support the battery level property.</returns>
        [DllImport(LibMtpName)]
        private static extern int LIBMTP_Get_Batterylevel(IntPtr mtpDeviceStructPointer,
            ref byte maxLevel,
            ref byte currentLevel);

        public static int GetBatteryLevel(IntPtr mtpDeviceStructPointer, ref byte maxLevel, ref byte currentLevel)
        {
            return LIBMTP_Get_Batterylevel(mtpDeviceStructPointer, ref maxLevel, ref currentLevel);
        }
        
        [DllImport(LibMtpName)]
        private static extern void LIBMTP_Dump_Errorstack(IntPtr mtpDeviceStructPointer);

        public static void DumpErrorStack(IntPtr device)
        {
            LIBMTP_Dump_Errorstack(device);
        }
    }
}