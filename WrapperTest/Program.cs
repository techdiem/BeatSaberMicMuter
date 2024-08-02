// See https://aka.ms/new-console-template for more information
using WrapperTest;

Console.WriteLine("Hello, World!");
bool muted = MicTest.GetMuteNative();
MicTest.GetDeviceList();
Console.WriteLine();
MicTest.SetMuteNative(!muted);
Console.ReadKey();