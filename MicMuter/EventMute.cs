using System;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using MicMuter.Configuration;
using BS_Utils.Utilities;

namespace MicMuter {
    public static class EventMute {
        public static MultiplayerSessionManager SessionManager { get; private set; }
        private static bool onlineActivated = false;
        private static bool mpconnected = false;

        public static void Setup() {
            BSEvents.menuSceneActive += OnSongExited;
            BSEvents.gameSceneActive += OnSongStarted;
            BSEvents.songPaused += OnGamePause;
            BSEvents.songUnpaused += OnGameResume;
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
            BSEvents.menuSceneActive -= OnSongExited;
            BSEvents.gameSceneActive -= OnSongStarted;
            BSEvents.songPaused -= OnGamePause;
            BSEvents.songUnpaused -= OnGameResume;

            if (SessionManager != null) {
                SessionManager.connectedEvent -= OnMultiplayerConnected;
                SessionManager.disconnectedEvent -= OnMultiplayerDisconnected;
            }
        }

        private static T FindFirstOrDefaultOptional<T>() where T : UnityEngine.Object {
            T obj = Resources.FindObjectsOfTypeAll<T>().FirstOrDefault();
            return obj;
        }

        private static void OnSongStarted() {
            if ((PluginConfig.Instance.MultiEnabled && mpconnected) ||
                (PluginConfig.Instance.Enabled && !mpconnected)) {
                MicDeviceUtils.SetMicMute(true);
            }
        }

        private static void OnSongExited() {
            if ((PluginConfig.Instance.MultiEnabled && mpconnected) ||
                (PluginConfig.Instance.Enabled && !mpconnected)) {
                MicDeviceUtils.SetMicMute(false);
            }
        }

        private static void OnGamePause() {
            if (PluginConfig.Instance.UnmuteOnPause) {
                MicDeviceUtils.SetMicMute(false);
            }
        }

        private static void OnGameResume() {
            if (PluginConfig.Instance.UnmuteOnPause) {
                MicDeviceUtils.SetMicMute(true);
            }
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
