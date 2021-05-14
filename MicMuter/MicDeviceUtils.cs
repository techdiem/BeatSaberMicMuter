using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSCore.CoreAudioAPI;
using MicMuter.Configuration;

namespace MicMuter {
    class MicDeviceUtils {
        //GUI variables
        public static Dictionary<string, string> micDeviceList = new Dictionary<string, string>();
        public static List<object> micSelectOptions = new List<object>();

        //CoreAudioApi device and event guid
        public static MMDevice microphone;
        public static Guid eventguid;

        public static void Setup() {
            //Generate GUID to identify the plugin's interaction with the CoreAudioAPI
            eventguid = Guid.NewGuid();

            //Load microphone device
            UpdateMicrophoneList();
            SelectConfiguredMic(PluginConfig.Instance.MicDeviceID);
        }

        public static void UpdateMicrophoneList() {
            MMDeviceEnumerator enumerator = new MMDeviceEnumerator();
            micSelectOptions.Clear();
            foreach (var mic in enumerator.EnumAudioEndpoints(DataFlow.Capture, DeviceState.Active)) {
                micSelectOptions.Add(mic.FriendlyName);
                micDeviceList.Add(mic.FriendlyName, mic.DeviceID);
            }
        }

        public static void SelectConfiguredMic(string micDeviceID) {
            //Get microphone devices
            MMDeviceEnumerator enumerator = new MMDeviceEnumerator();
            var devices = enumerator.EnumAudioEndpoints(DataFlow.Capture, DeviceState.Active);


            //Load microphone id from config
            if (micDeviceID == "") {
                microphone = devices.FirstOrDefault();
                PluginConfig.Instance.MicDeviceID = microphone.DeviceID;
                Plugin.Log.Info($"No device configured, using default: {microphone.FriendlyName}");
            }
            else if (microphone != null) {
                if (microphone.DeviceID != micDeviceID) {
                    microphone = enumerator.GetDevice(micDeviceID);
                    Plugin.Log.Info($"Switching device to {microphone.FriendlyName}");
                }
            }
            else {
                microphone = enumerator.GetDevice(micDeviceID);
                Plugin.Log.Info($"Using device from config: {microphone.FriendlyName}");
            }
        }

        public static void SetMicMute(bool muted) {
            var endpoint = AudioEndpointVolume.FromDevice(microphone);
            if (endpoint.GetMute() != muted) {
                endpoint.SetMute(muted, eventguid);
                Plugin.Log.Info($"Microphone muted: {muted}");
            }
        }
    }
}
