using System;
using System.Linq;
using UnityEngine;
using MicMuter.Configuration;
using MicMuter.UI;
using BS_Utils.Utilities;

namespace MicMuter {
    public static class EventMute {
        public static MultiplayerSessionManager SessionManager { get; private set; }
        private static bool onlineActivated = false;
        private static bool mpconnected = false;

        public static void Setup() {
            //Automatic mute
            BSEvents.menuSceneActive += OnSongExited;
            BSEvents.gameSceneActive += OnSongStarted;
            BSEvents.songPaused += OnGamePause;
            BSEvents.songUnpaused += OnGameResume;
            //Ptt controller buttons
            ControllersHelper.LeftTriggerChanged += OnLeftTriggerChange;
            ControllersHelper.RightTriggerChanged += OnRightTriggerChange;
            ControllersHelper.LeftGripChanged += OnLeftGripChange;
            ControllersHelper.RightGripChanged += OnRightGripChange;
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

        private static void OnSongStarted() {
            if ((PluginConfig.Instance.MultiEnabled && mpconnected) ||
                (PluginConfig.Instance.SingleEnabled && !mpconnected)) {
                MicDeviceUtils.SetMicMute(true);
            }
        }

        private static void OnSongExited() {
            if ((PluginConfig.Instance.MultiEnabled && mpconnected) ||
                (PluginConfig.Instance.SingleEnabled && !mpconnected)) {
                MicDeviceUtils.SetMicMute(false);
            }
        }

        private static void OnGamePause() {
            if (PluginConfig.Instance.UnmuteOnPause && PluginConfig.Instance.SingleEnabled) {
                MicDeviceUtils.SetMicMute(false);
            }
        }

        private static void OnGameResume() {
            if (PluginConfig.Instance.UnmuteOnPause && PluginConfig.Instance.SingleEnabled) {
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

        private static bool? ChooseMuteStateFromButton(bool pressed)
        {
            if (PluginConfig.Instance.PTTActionMode == "Push-to-talk")
            {
                //Opposite of the button state -> pressed button = mute false
                return !pressed;
            }
            else if (PluginConfig.Instance.PTTActionMode == "Push-to-mute")
            {
                //Same state as button -> pressed button = mute true
                return pressed;
            }
            else if (pressed)
            {
                //Push-to-toggle, opposite of the current status
                //Only work on button press=true, because releasing the button is irrelevant here
                //Return the opposite of the current status
                return !MicDeviceUtils.GetMuteStatus();
            }
            return null;
        }

        private static void OnLeftTriggerChange(bool state) {
            if (PluginConfig.Instance.PTTKey == "L Trigger" || (PluginConfig.Instance.PTTKey == "L+R Trigger" && ControllersHelper.RightTriggerState == state)) {
                bool? newState = ChooseMuteStateFromButton(state);
                if (newState != null)
                {
                    MicDeviceUtils.SetMicMute(newState.GetValueOrDefault());
                    MuteButtonWindowController.Instance.UpdateMutebtn();
                }
            }
        }

        private static void OnRightTriggerChange(bool state) {
            if (PluginConfig.Instance.PTTKey == "R Trigger" || (PluginConfig.Instance.PTTKey == "L+R Trigger" && ControllersHelper.LeftTriggerState == state)) {
                bool? newState = ChooseMuteStateFromButton(state);
                if (newState != null)
                {
                    MicDeviceUtils.SetMicMute(newState.GetValueOrDefault());
                    MuteButtonWindowController.Instance.UpdateMutebtn();
                }
            }
        }

        private static void OnLeftGripChange(bool state) {
            if (PluginConfig.Instance.PTTKey == "L Grip" || (PluginConfig.Instance.PTTKey == "L+R Grip" && ControllersHelper.RightGripState == state)) {
                bool? newState = ChooseMuteStateFromButton(state);
                if (newState != null)
                {
                    MicDeviceUtils.SetMicMute(newState.GetValueOrDefault());
                    MuteButtonWindowController.Instance.UpdateMutebtn();
                }
            }
        }

        private static void OnRightGripChange(bool state) {
            if (PluginConfig.Instance.PTTKey == "R Grip" || (PluginConfig.Instance.PTTKey == "L+R Grip" && ControllersHelper.LeftGripState == state)) {
                bool? newState = ChooseMuteStateFromButton(state);
                if (newState != null)
                {
                    MicDeviceUtils.SetMicMute(newState.GetValueOrDefault());
                    MuteButtonWindowController.Instance.UpdateMutebtn();
                }
            }
        }
    }
}
