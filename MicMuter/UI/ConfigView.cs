using System;
using System.Linq;
using System.Collections.Generic;
using BeatSaberMarkupLanguage;
using BeatSaberMarkupLanguage.Attributes;
using MicMuter.Configuration;

namespace MicMuter.UI {
    public class ConfigView : PersistentSingleton<ConfigView> { 

        private PluginConfig _settings = PluginConfig.Instance;

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

        [UIValue("multi-mute")]
        protected bool MultiMuteEnabled {
            get => _settings.MultiMuteEnabled;
            set => _settings.MultiMuteEnabled = value;
        }

        [UIValue("micdevice-value")]
        protected object MicDeviceValue {
            get {
                if (!string.IsNullOrEmpty(_settings.MicDeviceID)) {
                    string deviceName = MicDeviceUtils.micDeviceList.FirstOrDefault(x => x.Value == _settings.MicDeviceID).Key;
                    return (object)deviceName;
                }
                else {
                    return "";
                }
            }
            set {
                _settings.MicDeviceID = MicDeviceUtils.micDeviceList[value as string];
            }
        }

        [UIValue("micdevice-options")]
        public List<object> micSelectOptions = MicDeviceUtils.micSelectOptions;
    }
}
