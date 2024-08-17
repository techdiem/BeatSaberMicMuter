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
            micDeviceList.Clear();
            var devices = enumerator.EnumAudioEndpoints(DataFlow.Capture, DeviceState.Active);
            if (devices.Count > 0)
            {
                foreach (var mic in devices)
                {
                    micSelectOptions.Add(mic.FriendlyName);
                    micDeviceList.Add(mic.FriendlyName, mic.DeviceID);
                }
            }
            else
            {
                micSelectOptions.Add("No devices");
                micDeviceList.Add("No devices", "");
            }
        }

        public static void SelectConfiguredMic(string micDeviceID) {
            //Get microphone devices
            MMDeviceEnumerator enumerator = new MMDeviceEnumerator();
            var devices = enumerator.EnumAudioEndpoints(DataFlow.Capture, DeviceState.Active);


            //Load microphone id from config
            if (micDeviceID == "")
            {
                microphone = devices.FirstOrDefault();
                PluginConfig.Instance.MicDeviceID = microphone.DeviceID;
                Plugin.Log.Info($"No device configured, using default: {microphone.FriendlyName}");
            }
            else
            {
                bool devicePresent = false;
                foreach (var device in devices) 
                {
                    if (device.DeviceID == micDeviceID) 
                    { 
                        devicePresent = true;
                        break;
                    }
                }
                if (devicePresent)
                {
                    microphone = enumerator.GetDevice(micDeviceID);
                    if (microphone.DeviceID == micDeviceID)
                    {
                        Plugin.Log.Info($"Using device from config: {microphone.FriendlyName}");
                    }
                    else
                    {
                        Plugin.Log.Info($"Switching device to {microphone.FriendlyName}");
                    }
                }
                else
                {
                    if (devices.Count > 0)
                    {
                        Plugin.Log.Error("Microphone from configuration is not present on system, switching to default device.");
                        microphone = devices.FirstOrDefault();
                    }
                    else
                    {
                        Plugin.Log.Error("No microphones present on this system, check your devices.");
                    }
                }
            }
        }

        public static void SetMicMute(bool muted)
        {
            if (microphone != null) {
                var endpoint = AudioEndpointVolume.FromDevice(microphone);
                if (endpoint.GetMute() != muted)
                {
                    endpoint.SetMute(muted, eventguid);
                    Plugin.Log.Info($"Microphone muted: {muted}");
                }
            }
        }

        public static bool GetMuteStatus() {
            if (microphone != null)
            {
                var endpoint = AudioEndpointVolume.FromDevice(microphone);
                return endpoint.GetMute();
            }
            else
            {
                return false;
            }
        }
    }
}
