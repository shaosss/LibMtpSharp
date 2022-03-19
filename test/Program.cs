// See https://aka.ms/new-console-template for more information

using LibMtpSharp.Lists;

var list = new RawDeviceList();
foreach(var device in list)
    Console.WriteLine(device);