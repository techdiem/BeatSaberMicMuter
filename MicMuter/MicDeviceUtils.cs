using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSCore.CoreAudioAPI;
using MicMuter.Configuration;

namespace MicMuter {
    class MicDeviceUtils {
        public static Dictionary<string, string> micDeviceList = new Dictionary<string, string>();
        public static List<object> micSelectOptions = new List<object>();

        public static void UpdateMicrophoneList() {
            MMDeviceEnumerator enumerator = new MMDeviceEnumerator();
            micSelectOptions.Clear();
            foreach (var mic in enumerator.EnumAudioEndpoints(DataFlow.Capture, DeviceState.Active)) {
                micSelectOptions.Add(mic.FriendlyName);
                micDeviceList.Add(mic.FriendlyName, mic.DeviceID);
            }
        }

        public static void SelectConfiguredMic(string micDeviceID) {
            MMDevice selectedMic;
            //Get microphone devices
            MMDeviceEnumerator enumerator = new MMDeviceEnumerator();
            var devices = enumerator.EnumAudioEndpoints(DataFlow.Capture, DeviceState.Active);


            //Load microphone id from config
            if (micDeviceID == "") {
                selectedMic = devices.FirstOrDefault();
                PluginConfig.Instance.MicDeviceID = selectedMic.DeviceID;
                Plugin.Log.Info("No device configured, using default");
            }
            else {
                selectedMic = enumerator.GetDevice(micDeviceID);
                Plugin.Log.Info("Using device from config");
            }
            Plugin.microphone = selectedMic;
            Plugin.Log.Info(selectedMic.FriendlyName);
        }
    }
}
