using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSCore.CoreAudioAPI;
using MicMuter.Configuration;

namespace MicMuter {
    class MicDeviceUtils {
        private PluginConfig _settings = PluginConfig.Instance;
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

    }
}
