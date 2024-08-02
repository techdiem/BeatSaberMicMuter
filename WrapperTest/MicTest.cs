using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace WrapperTest
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct DevDetails
    {
        public IntPtr name;
        public IntPtr id;

        public string Name
        {
            get { return Marshal.PtrToStringUni(name); }
        }

        public string ID
        {
            get { return Marshal.PtrToStringUni(id); }
        }
    }

    public class MicTest
    {
        [DllImport("CoreAudioWrapper.dll", CallingConvention = CallingConvention.StdCall)]
        private static extern bool GetMute([MarshalAsAttribute(UnmanagedType.LPWStr)] string deviceID);

        [DllImport("CoreAudioWrapper.dll", CallingConvention = CallingConvention.StdCall)]
        private static extern bool SetMute([MarshalAsAttribute(UnmanagedType.Bool)] bool mute, [MarshalAsAttribute(UnmanagedType.LPWStr)] string deviceID);

        [DllImport("CoreAudioWrapper.dll", CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.LPWStr)]
        private static extern string GetDefaultDeviceID();

        [DllImport("CoreAudioWrapper.dll", CallingConvention = CallingConvention.StdCall)]
        private static extern IntPtr GetMicrophoneList(out int devcount);

        public static bool GetMuteNative()
        {
            string deviceID = GetDefaultDeviceID();
            Console.WriteLine(deviceID);
            bool muted = GetMute(deviceID);
            Console.WriteLine(muted.ToString());
            return muted;
        }

        public static void SetMuteNative(bool muted)
        {
            string deviceID = GetDefaultDeviceID();
            Console.WriteLine(deviceID);
            SetMute(muted, deviceID);
            Console.Write(muted.ToString());
        }

        public static DevDetails[] GetManagedDevDetailsArray()
        {
            int count;
            IntPtr ptr = GetMicrophoneList(out count);
            int size = Marshal.SizeOf(typeof(DevDetails));
            DevDetails[] array = new DevDetails[count];
            for (int i = 0; i < count; i++)
            {
                IntPtr current = new IntPtr(ptr.ToInt64() + i * size);
                array[i] = (DevDetails)Marshal.PtrToStructure(current, typeof(DevDetails));
            }

            return array;
        }

        public static void GetDeviceList()
        {
            DevDetails[] devDetails = GetManagedDevDetailsArray();
            foreach (DevDetails dev in devDetails) {
                Console.WriteLine(dev.ID);
                Console.WriteLine(dev.Name);
            }
        }
    }
}
