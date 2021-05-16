using IPA;
using IPA.Config;
using IPA.Config.Stores;
using System;
using System.Linq;
using System.Reflection;
using IPALogger = IPA.Logging.Logger;
using MicMuter.Configuration;
using MicMuter.UI;
using BeatSaberMarkupLanguage.Settings;

namespace MicMuter {
    [Plugin(RuntimeOptions.SingleStartInit)]
    public class Plugin {
        public const string HarmonyId = "mod.micmuter";
        internal static HarmonyLib.Harmony Harmony { get; private set; }
        internal static Plugin Instance { get; private set; }
        internal static IPALogger Log { get; private set; }
        public static bool startupReady = false;

        [Init]
        public void Init(IPALogger logger, IPA.Config.Config conf) {
            Instance = this;
            Log = logger;
            PluginConfig.Instance = conf.Generated<PluginConfig>();
            BS_Utils.Utilities.BSEvents.lateMenuSceneLoadedFresh += BSEvents_lateMenuSceneLoadedFresh;
        }

        [OnStart]
        public void OnApplicationStart() {
            //Harmony
            Harmony = new HarmonyLib.Harmony(HarmonyId);
            Harmony.PatchAll(Assembly.GetExecutingAssembly());
            
            //new GameObject("MicMuterController").AddComponent<MicMuterController>();
            
            //Gameplay events to mute/unmute
            EventMute.Setup();
            
            //Register mod settings menu button
            BSMLSettings.instance.AddSettingsMenu("MicMuter", "MicMuter.UI.ConfigView.bsml", ConfigView.instance);

            //Microphone device setup
            MicDeviceUtils.Setup();

        }

        private void BSEvents_lateMenuSceneLoadedFresh(ScenesTransitionSetupDataSO obj) {
            startupReady = true;
            if (PluginConfig.Instance.ScreenEnabled) {
                //Floating mute button
                MuteButtonWindowController.Instance.ShowMuteWindow();
            }
        }

        [OnExit]
        public void OnApplicationQuit() {
            EventMute.Cleanup();
            MuteButtonWindowController.Instance.Cleanup();
        }
    }
}
