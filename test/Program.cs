// See https://aka.ms/new-console-template for more information

var list = new LibMtpSharp.RawDeviceList();
foreach(var device in list)
    Console.WriteLine(device);