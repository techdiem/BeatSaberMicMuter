using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Components;
using BeatSaberMarkupLanguage.ViewControllers;
using TMPro;

namespace MicMuter.UI {
    [HotReload(@"MuteButtonWindow.bsml")]
    public partial class MuteButtonWindow : BSMLAutomaticViewController {
        internal MuteButtonWindowController ParentCoordinator;

        [UIComponent("mutebtn")]
        private TextMeshProUGUI mutebtnText;

        [UIAction("togglemute")]
        protected void ClickButtonAction() {
            bool mutestatus = MicDeviceUtils.GetMuteStatus();
            MicDeviceUtils.SetMicMute(!mutestatus);
            UpdateMutebtnText();
        }

        public void UpdateMutebtnText() {
            bool mutestatus = MicDeviceUtils.GetMuteStatus();
            if (mutestatus) {
                mutebtnText.text = "Unmute";
            }
            else {
                mutebtnText.text = "Mute";
            }
        }
    }
}
