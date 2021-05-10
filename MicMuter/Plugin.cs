using IPA;
using IPA.Config;
using IPA.Config.Stores;
using MicMuter.Configuration;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using IPALogger = IPA.Logging.Logger;
using MicMuter.UI;
using BeatSaberMarkupLanguage;
using BeatSaberMarkupLanguage.Settings;
using HMUI;

namespace MicMuter {
    [Plugin(RuntimeOptions.SingleStartInit)]
    public class Plugin {
        internal static Plugin Instance { get; private set; }
        internal static IPALogger Log { get; private set; }
        private PauseController pauseController;

        [Init]
        public void Init(IPALogger logger, IPA.Config.Config conf) {
            Instance = this;
            Log = logger;
            PluginConfig.Instance = conf.Generated<PluginConfig>();
        }

        [OnStart]
        public void OnApplicationStart() {
            new GameObject("MicMuterController").AddComponent<MicMuterController>();
            //Scene changed event to mute/unmute
            SceneManager.activeSceneChanged += OnActiveSceneChanged;

            //Register mod settings menu button
            BSMLSettings.instance.AddSettingsMenu("MicMuter", "MicMuter.UI.ConfigView.bsml", ConfigView.instance);

            //Microphone device setup
            MicDeviceUtils.Setup();
        }

        [OnExit]
        public void OnApplicationQuit() {
            SceneManager.activeSceneChanged -= OnActiveSceneChanged;
        }

        public void OnActiveSceneChanged(Scene oldscene, Scene newScene) {
            Log.Info(newScene.name);

            if (PluginConfig.Instance.Enabled) {
                if (newScene.name == "MenuCore") {
                    MicDeviceUtils.SetMicMute(false);
                    OnSongExited();

                }
                else if (newScene.name == "GameCore") {
                    MicDeviceUtils.SetMicMute(true);
                    OnSongStarted();
                }
            }
        }

        private static T FindFirstOrDefaultOptional<T>() where T : UnityEngine.Object {
            T obj = Resources.FindObjectsOfTypeAll<T>().FirstOrDefault();
            return obj;
        }

        public void OnSongStarted() {
            pauseController = FindFirstOrDefaultOptional<PauseController>();

            //no pauseController in multiplayer
            if (pauseController != null && PluginConfig.Instance.UnmuteOnPause) {
                pauseController.didPauseEvent += OnGamePause;
                pauseController.didResumeEvent += OnGameResume;
            }
        }

        public void OnSongExited() {
            //no pauseController in multiplayer
            if (pauseController != null && PluginConfig.Instance.UnmuteOnPause) {
                pauseController.didPauseEvent -= OnGamePause;
                pauseController.didResumeEvent -= OnGameResume;
            }
        }

        public void OnGamePause() {
            Log.Info("GamePause");
            MicDeviceUtils.SetMicMute(false);
        }
        public void OnGameResume() {
            Log.Info("GameResume");
            MicDeviceUtils.SetMicMute(true);
        }
    }
}
