using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using MicMuter.Configuration;

namespace MicMuter {

    #region Microphone device details struct
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
    #endregion

    class MicDeviceUtils {
        #region Native c++ dll import
        [DllImport("CoreAudioWrapper.dll", CallingConvention = CallingConvention.StdCall)]
        private static extern bool GetMute([MarshalAsAttribute(UnmanagedType.LPWStr)] string deviceID);

        [DllImport("CoreAudioWrapper.dll", CallingConvention = CallingConvention.StdCall)]
        private static extern bool SetMute([MarshalAsAttribute(UnmanagedType.Bool)] bool mute, [MarshalAsAttribute(UnmanagedType.LPWStr)] string deviceID);

        [DllImport("CoreAudioWrapper.dll", CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.LPWStr)]
        private static extern string GetDefaultDeviceID();

        [DllImport("CoreAudioWrapper.dll", CallingConvention = CallingConvention.StdCall)]
        private static extern IntPtr GetMicrophoneList(out int devcount);
        #endregion


        //GUI variables
        public static Dictionary<string, string> micDeviceList = new Dictionary<string, string>();
        public static List<object> micSelectOptions = new List<object>();
        private static string micDeviceID;

        public static void Setup() {

            //Load microphone device
            UpdateMicrophoneList();
            SelectConfiguredMic(PluginConfig.Instance.MicDeviceID);
        }

        public static void UpdateMicrophoneList() {
            DevDetails[] microphones = GetDeviceListNative();

            micSelectOptions.Clear();
            foreach (var mic in microphones) {
                micSelectOptions.Add(mic.Name);
                micDeviceList.Add(mic.Name, mic.ID);
            }
        }

        public static void SelectConfiguredMic(string devID) {
            //Load microphone id from config
            if (devID == "")
            {
                string defaultID = GetDefaultDeviceID();
                PluginConfig.Instance.MicDeviceID = defaultID;
                micDeviceID = defaultID;
                Plugin.Log.Info($"No device configured, using default mic with id: {defaultID}");
            }
            else if (micDeviceID != devID)
            {
                micDeviceID = devID;
                Plugin.Log.Info($"Switching device to {devID}");
            }
        }

        public static void SetMicMute(bool muted) {
            if (GetMute(micDeviceID) != muted) {
                SetMute(muted, micDeviceID);
                Plugin.Log.Info($"Microphone muted: {muted}");
            }
        }

        public static bool GetMuteStatus() {
            return GetMute(micDeviceID);
        }


        #region Native call to c++ lib
        public static DevDetails[] GetDeviceListNative()
        {
            int count;
            IntPtr ptr = GetMicrophoneList(out count);
            int size = Marshal.SizeOf(typeof(DevDetails));
            DevDetails[] array = new DevDetails[count];
            try
            {
                for (int i = 0; i < count; i++)
                {
                    IntPtr current = new IntPtr(ptr.ToInt64() + i * size);
                    array[i] = (DevDetails)Marshal.PtrToStructure(current, typeof(DevDetails));
                }
            }
            catch (Exception e)
            {
                Plugin.Log.Error($"Error processing native audioapi calls: {e}");
            }

            return array;
        }
        #endregion
    }
}
