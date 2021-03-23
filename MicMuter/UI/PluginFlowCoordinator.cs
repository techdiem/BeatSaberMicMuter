using System;
using BeatSaberMarkupLanguage;
using HMUI;

namespace MicMuter.UI {
    public class PluginFlowCoordinator : FlowCoordinator {
        private ConfigViewController configView;

        void Awake() {
            configView = BeatSaberUI.CreateViewController<ConfigViewController>();
            configView.UpdateMicrophoneList();
        }

        protected override void DidActivate(bool firstActivation, bool addedToHierarchy, bool screenSystemEnabling) {
            if (firstActivation) {
                SetTitle("MicMuter");
                showBackButton = true;
                ProvideInitialViewControllers(configView);
            }
        }

        protected override void BackButtonWasPressed(ViewController topViewController) {
            BeatSaberUI.MainFlowCoordinator.DismissFlowCoordinator(this);
        }
    }
}
