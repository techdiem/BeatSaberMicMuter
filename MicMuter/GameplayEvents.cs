using System;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using MicMuter.Configuration;

namespace MicMuter {
    public static class GameplayEvents {

        private static PauseController pauseController;
        public static MultiplayerSessionManager SessionManager { get; private set; }
        private static bool onlineActivated = false;
        private static bool mpconnected = false;

        public static void Setup() {
            SceneManager.activeSceneChanged += OnActiveSceneChanged;
        }

        public static void SetupMP() {
            if (onlineActivated) {
                return;
            }

            //Get Multiplayer Session Manager
            SessionManager = Resources.FindObjectsOfTypeAll<MultiplayerSessionManager>().FirstOrDefault();

            if (SessionManager == null) {
                Plugin.Log.Critical("Can't get Multiplayer SessionManager!");
                return;
            }

            SessionManager.connectedEvent += OnMultiplayerConnected;
            SessionManager.disconnectedEvent += OnMultiplayerDisconnected;

            onlineActivated = true;
        }

        public static void Cleanup() {
            SceneManager.activeSceneChanged -= OnActiveSceneChanged;

            if (SessionManager != null) {
                SessionManager.connectedEvent -= OnMultiplayerConnected;
                SessionManager.disconnectedEvent -= OnMultiplayerDisconnected;
            }
        }

        private static T FindFirstOrDefaultOptional<T>() where T : UnityEngine.Object {
            T obj = Resources.FindObjectsOfTypeAll<T>().FirstOrDefault();
            return obj;
        }

        private static void OnActiveSceneChanged(Scene oldscene, Scene newScene) {
            Plugin.Log.Debug(newScene.name);

            if ((PluginConfig.Instance.MultiEnabled && mpconnected) ||
                (PluginConfig.Instance.Enabled && !mpconnected)) {
                if (newScene.name == "MenuCore") {
                    OnSongExited();

                }
                else if (newScene.name == "GameCore") {
                    OnSongStarted();
                }
            }
        }

        private static void OnSongStarted() {
            MicDeviceUtils.SetMicMute(true);

            pauseController = FindFirstOrDefaultOptional<PauseController>();

            //no pauseController in multiplayer
            if (pauseController != null && PluginConfig.Instance.UnmuteOnPause) {
                pauseController.didPauseEvent += OnGamePause;
                pauseController.didResumeEvent += OnGameResume;
            }
        }

        private static void OnSongExited() {
            MicDeviceUtils.SetMicMute(false);

            if (pauseController != null && PluginConfig.Instance.UnmuteOnPause) {
                pauseController.didPauseEvent -= OnGamePause;
                pauseController.didResumeEvent -= OnGameResume;
            }
        }

        private static void OnGamePause() {
            Plugin.Log.Debug("GamePause");
            MicDeviceUtils.SetMicMute(false);
        }
        private static void OnGameResume() {
            Plugin.Log.Debug("GameResume");
            MicDeviceUtils.SetMicMute(true);
        }

        private static void OnMultiplayerConnected() {
            Plugin.Log.Debug("Multiplayer connected");
            //Using events instead of SessionManager.isconnected() because it
            //becomes easier in the condition since the SessionManager can be null
            mpconnected = true;
        }
        private static void OnMultiplayerDisconnected(DisconnectedReason reason) {
            Plugin.Log.Debug("Multiplayer disconnected");
            mpconnected = false;
        }
    }
}
