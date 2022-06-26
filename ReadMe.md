# LibMtpSharp

LibMtpSharp is a wrapper around [libmtp library](https://github.com/libmtp/libmtp) written in c with a bit of custom changes
at [this fork](https://github.com/shaosss/libmtp) which made for being able to wrap c code properly
and some ease of use functionality.

## What packages does I need to use?

There are multiple packages related to the wrapper: the wrapper itself and packages with native libraries.
To use the wrapper you need LibMtpSharp package and appropriate package for the OS which your SW targets.
You can see all the packages and differences in the table below:

| Package name                                | Latest Version | Content                                                                                                                | Usage Instructions                                                          |
|---------------------------------------------|----------------|------------------------------------------------------------------------------------------------------------------------|-----------------------------------------------------------------------------|
| LibMtpSharp                                 | 0.1.3          | Contains the wrapper managed code. API coverage is limited and will improve in future versions.                        | Main package with logic. Required to be able to use libmpt                  |
| LibMtpSharp.Native.Linux                    | 1.1.20-beta    | Linux native Libmtp library with custom improvements built for x64                                                     | Use if you target linux and instruct the user how to install dependencies   |
| LibMtpSharp.Native.Linux.WithDependencies   | 1.1.20-beta    | Linux native dependencies built for x64 and references LibMtpSharp.Native.Linux package.                               | Use if you target linux and don't want user to manage dependencies          |
| LibMtpSharp.Native.MacOS                    | 1.1.20-beta    | MacOS native Libmtp library with custom improvements built for x64 (not sure if will work on M1)                       | Use if you target MacOS and instruct the user how to install dependencies   |
| LibMtpSharp.Native.MacOS.WithDependencies   | 1.1.20-beta    | MacOS native dependencies built for x64 (not sure if will work on M1) and references LibMtpSharp.Native.MacOS package. | Use if you target MacOS and don't want user to manage dependencies          |
| LibMtpSharp.Native.Windows                  | 1.1.20-beta    | Windows native Libmtp library with custom improvements built for x64                                                   | Use if you target Windows and instruct the user how to install dependencies |
| LibMtpSharp.Native.Windows.WithDependencies | 1.1.20-beta    | Windows native dependencies built for x64 and references LibMtpSharp.Native.Windows package.                           | Use if you target Windows and don't want user to manage dependencies        |

The dependencies package include following libraries: libgcrypt, libgpg-error, libiconv, libcharset and libusb.

## What has been changed in native libmtp?

The libmtp native library in the packages contains followinf changes:
- Add `LIBMTP_Free(void *)` function to free native resources (.net can't access c `free()` function directly, since it's behaviour can be compiler specific)
- Add bcd device info to `DeviceEntry` struct.
- Make MTPZ data be able to come from shared resources, not only from file in $HOME directory as used in vanila libmtp library.

## How to use wrapper?

The documentation is limited for now, but you can use LibMtp library documentation for reference. The documentation will be improved with time.

To get the list of available devices you should create `RawDeviceList`, which implements the `IEnumerable<RawDevice>`. **!Dont forget to dispose it!**

```c#
using (var list = new RawDeviceList())
{
    foreach(var device in list)
        Console.WriteLine(device);
}
````

To connect to device in interest you should create the `OpenedMtpDevice` with corresponding `RawDevice` struct from the list

```c#
using (var list = new RawDeviceList())
{
    var rawDevice = list.First(); //assuming we have at least one device
    var connectedDevice = new OpenedMtpDevice(ref rawDevice, false);
}
```

`OpenedMtpDevice` contains the methods to communicate with device. **! `OpenedMtpDevice` is a disposable object. When you finished communicating with device - you should Dispose the instance. This is equivalent to closing the connection. !**

You can donate through Github or [Paypal](https://www.paypal.com/donate/?hosted_button_id=FFM78JRJCKNS8)
