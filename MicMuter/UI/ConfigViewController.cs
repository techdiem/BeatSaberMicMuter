using System;
using System.Linq;
using System.Collections.Generic;
using BeatSaberMarkupLanguage;
using BeatSaberMarkupLanguage.ViewControllers;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Components.Settings;
using BeatSaberMarkupLanguage.Parser;
using MicMuter.Configuration;
using CSCore.CoreAudioAPI;

namespace MicMuter.UI {
    [ViewDefinition("MicMuter.UI.Views.ConfigView.bsml")]
    public class ConfigViewController : BSMLAutomaticViewController {

        private PluginConfig _settings = PluginConfig.Instance;
        private Dictionary<string, string> micDeviceList = new Dictionary<string, string>();

        public void UpdateMicrophoneList() {
            MMDeviceEnumerator enumerator = new MMDeviceEnumerator();
            micSelectOptions.Clear();
            foreach (var mic in enumerator.EnumAudioEndpoints(DataFlow.Capture, DeviceState.Active)) {
                micSelectOptions.Add(mic.FriendlyName);
                micDeviceList.Add(mic.FriendlyName, mic.DeviceID);
            }
        }

        [UIValue("enabled")]
        protected bool Enabled {
            get => _settings.Enabled;
            set => _settings.Enabled = value;
        }

        [UIValue("pause-unmute")]
        protected bool UnmuteOnPause {
            get => _settings.UnmuteOnPause;
            set => _settings.UnmuteOnPause = value;
        }

        [UIValue("micdevice-value")]
        protected object micDeviceValue {
            get {
                if (!string.IsNullOrEmpty(_settings.MicDeviceID)) {
                    string deviceName = micDeviceList.FirstOrDefault(x => x.Value == _settings.MicDeviceID).Key;
                    return (object)deviceName;
                }
                else {
                    return "";
                }
            }
            set {
                _settings.MicDeviceID = micDeviceList[value as string];
            }
        }

        [UIValue("micdevice-options")]
        public List<object> micSelectOptions = new List<object>();
    }
}
