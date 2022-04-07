// See https://aka.ms/new-console-template for more information

using LibMtpSharp;

using var list = new RawDeviceList();
foreach(var device in list)
    Console.WriteLine(device);