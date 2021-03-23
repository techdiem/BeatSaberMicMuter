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
using CSCore.CoreAudioAPI;
using MicMuter.UI;
using BeatSaberMarkupLanguage;
using BeatSaberMarkupLanguage.MenuButtons;
using HMUI;

namespace MicMuter {
    [Plugin(RuntimeOptions.SingleStartInit)]
    public class Plugin {
        internal static Plugin Instance { get; private set; }
        internal static IPALogger Log { get; private set; }
        private PauseController pauseController;
        public MenuButton mainMenuButton;
        public static FlowCoordinator flowCoordinator;

        //CoreAudioApi device and event guid
        public static MMDevice microphone;
        public static Guid eventguid;

        [Init]
        /// <summary>
        /// Called when the plugin is first loaded by IPA (either when the game starts or when the plugin is enabled if it starts disabled).
        /// [Init] methods that use a Constructor or called before regular methods like InitWithConfig.
        /// Only use [Init] with one Constructor.
        /// </summary>
        public void Init(IPALogger logger, IPA.Config.Config conf) {
            Instance = this;
            Log = logger;
            PluginConfig.Instance = conf.Generated<PluginConfig>();
            eventguid = Guid.NewGuid();
            
        }

        [OnStart]
        public void OnApplicationStart() {
            new GameObject("MicMuterController").AddComponent<MicMuterController>();
            //Scene changed event to mute/unmute
            SceneManager.activeSceneChanged += OnActiveSceneChanged;

            //Register main menu button
            mainMenuButton = new MenuButton("MicMuter", "MicMuter settings", onSettingsbtnpress, true);
            MenuButtons.instance.RegisterButton(mainMenuButton);

            //Get microphone devices
            MMDeviceEnumerator enumerator = new MMDeviceEnumerator();
            var devices = enumerator.EnumAudioEndpoints(DataFlow.Capture, DeviceState.Active);
            

            //Load microphone id from config
            if(PluginConfig.Instance.MicDeviceID == "") {
                microphone = devices.FirstOrDefault();
                PluginConfig.Instance.MicDeviceID = microphone.DeviceID;
                Log.Info("No device configured");
            }
            else {
                microphone = enumerator.GetDevice(PluginConfig.Instance.MicDeviceID);
                Log.Info("Using device from config");
            }
            Log.Info(microphone.FriendlyName);

        }

        internal void onSettingsbtnpress() {
            if (flowCoordinator == null) {
                flowCoordinator = BeatSaberUI.CreateFlowCoordinator<PluginFlowCoordinator>();
            }
            BeatSaberUI.MainFlowCoordinator.PresentFlowCoordinator(flowCoordinator);
        }

        [OnExit]
        public void OnApplicationQuit() {
            SceneManager.activeSceneChanged -= OnActiveSceneChanged;
        }

        public void OnActiveSceneChanged(Scene oldscene, Scene newScene) {
            Log.Info(newScene.name);

            if (PluginConfig.Instance.Enabled) {
                if (newScene.name == "MenuCore") {
                    SetMicMute(false);
                    OnSongExited();

                }
                else if (newScene.name == "GameCore") {
                    SetMicMute(true);
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
            SetMicMute(false);
        }
        public void OnGameResume() {
            Log.Info("GameResume");
            SetMicMute(true);
        }

        public void SetMicMute(bool muted) {
        var endpoint = AudioEndpointVolume.FromDevice(microphone);
        if (endpoint.GetMute() != muted) {
            endpoint.SetMute(muted, eventguid);
            Log.Info(muted.ToString());
        }
        }
    }
}
