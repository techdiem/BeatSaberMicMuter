using System;
using System.Linq;
using System.Collections.Generic;
using BeatSaberMarkupLanguage;
using BeatSaberMarkupLanguage.Attributes;
using MicMuter.Configuration;
using TMPro;

namespace MicMuter.UI {
    public class ConfigView : PersistentSingleton<ConfigView> { 

        private PluginConfig _settings = PluginConfig.Instance;

        [UIValue("single-enabled")]
        protected bool SingleEnabled {
            get => _settings.SingleEnabled;
            set => _settings.SingleEnabled = value;
        }

        [UIValue("pause-unmute")]
        protected bool UnmuteOnPause {
            get => _settings.UnmuteOnPause;
            set => _settings.UnmuteOnPause = value;
        }

        [UIValue("multi-enabled")]
        protected bool MultiEnabled {
            get => _settings.MultiEnabled;
            set => _settings.MultiEnabled = value;
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


        [UIValue("pttEnabled")]
        protected bool PttEnabled {
            get => _settings.PTTEnabled;
            set => _settings.PTTEnabled = value;
        }

        [UIValue("ptt-options")]
        protected List<object> pttComboOptions = new object[] { "L Trigger", "R Trigger", "L+R Trigger", "L Grip", "R Grip", "L+R Grip" }.ToList();

        [UIValue("ptt-value")]
        protected string pttCombo {
            get => _settings.PTTMode;
            set => _settings.PTTMode = value;
        }

        [UIValue("pttInverted")]
        protected bool PttInverted {
            get => _settings.PTTInverted;
            set => _settings.PTTInverted = value;
        }


        [UIValue("screenEnabled")]
        protected bool ScreenEnabled {
            get => _settings.ScreenEnabled;
            set { 
                _settings.ScreenEnabled = value;
                //Reset button text in case that the handle is activated but the gameobject gets removed
                toggleScreenHandleBtnText.text = "Show movement handle";
            }
        }


        [UIComponent("togglescreenhandlebtn")]
        private TextMeshProUGUI toggleScreenHandleBtnText;

        [UIAction("togglescreenhandle")]
        protected void ClickToggleButtonAction() {
            if (MuteButtonWindowController.Instance.MuteButtonScreen != null) {
                Plugin.Log.Debug("Toggle screen handle visibility");
                bool oldstate = MuteButtonWindowController.Instance.MuteButtonScreen.ShowHandle;
                MuteButtonWindowController.Instance.MuteButtonScreen.ShowHandle = !oldstate;
                if (oldstate) {
                    toggleScreenHandleBtnText.text = "Show movement handle";
                }
                else {
                    toggleScreenHandleBtnText.text = "Hide movement handle";
                }
            }
        }
    }
}
