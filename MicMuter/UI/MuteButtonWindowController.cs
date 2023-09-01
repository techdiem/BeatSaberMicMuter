using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BeatSaberMarkupLanguage;
using BeatSaberMarkupLanguage.Components;
using BeatSaberMarkupLanguage.FloatingScreen;
using VRUIControls;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using BS_Utils.Utilities;
using MicMuter.Configuration;

namespace MicMuter.UI {
    class MuteButtonWindowController {
        private static MuteButtonWindowController _instance;
        public static MuteButtonWindowController Instance {
            get {
                if (_instance == null)
                    _instance = new MuteButtonWindowController();
                return _instance;
            }
        }

        public FloatingScreen MuteButtonScreen;
        protected MuteButtonWindow MuteButtonWindow;
        protected MuteButtonWindowController() {
            BSEvents.earlyMenuSceneLoadedFresh += BSEvents_earlyMenuSceneLoadedFresh;
        }

        public void Cleanup() {
            BSEvents.earlyMenuSceneLoadedFresh -= BSEvents_earlyMenuSceneLoadedFresh;
            BSEvents.menuSceneActive -= OnSongExited;
            BSEvents.gameSceneActive -= OnSongStarted;
            BSEvents.songPaused -= OnGamePause;
            BSEvents.songUnpaused -= OnGameResume;

            if (MuteButtonScreen != null) {
                UnityEngine.Object.Destroy(MuteButtonScreen.gameObject);
                MuteButtonScreen = null;
            }
        }

        private void BSEvents_earlyMenuSceneLoadedFresh(ScenesTransitionSetupDataSO obj) {
            if (MuteButtonScreen != null) {
                UnityEngine.Object.Destroy(MuteButtonScreen.gameObject);
                MuteButtonScreen = null;
            }
        }

        public void ShowMuteWindow() {
            if (MuteButtonScreen == null) {
                MuteButtonScreen = CreateFloatingScreen();
                MuteButtonWindow = BeatSaberUI.CreateViewController<MuteButtonWindow>();
                MuteButtonWindow.ParentCoordinator = this;
                MuteButtonScreen.SetRootViewController(MuteButtonWindow, HMUI.ViewController.AnimationType.None);
                AttachEvents();
            }
            MuteButtonScreen.gameObject.SetActive(true);
            MuteButtonWindow.UpdateMutebtnText();
        }

        public FloatingScreen CreateFloatingScreen() {
            FloatingScreen screen = FloatingScreen.CreateFloatingScreen(
                new Vector2(20, 10), false,
                PluginConfig.Instance.ScreenPos,
                PluginConfig.Instance.ScreenRot);

            screen.HandleReleased -= OnRelease;
            screen.HandleReleased += OnRelease;

            UnityEngine.Object.DontDestroyOnLoad(screen.gameObject);
            return screen;
        }

        private void AttachEvents() {
            BSEvents.menuSceneActive += OnSongExited;
            BSEvents.gameSceneActive += OnSongStarted;
            BSEvents.songPaused += OnGamePause;
            BSEvents.songUnpaused += OnGameResume;
        }

        private void OnRelease(object _, FloatingScreenHandleEventArgs posRot) {
            Vector3 newPos = posRot.Position;
            Quaternion newRot = posRot.Rotation;

            PluginConfig.Instance.ScreenPos = newPos;
            PluginConfig.Instance.ScreenRot = newRot;
        }

        private void SetVisibility(bool visibility) {
            if (MuteButtonScreen != null) {
                MuteButtonScreen.gameObject.SetActive(visibility);
                if (visibility) {
                    MuteButtonWindow.UpdateMutebtnText();
                }
            }
        }

        public void UpdateMutebtn() {
            if (MuteButtonScreen != null) {
                MuteButtonWindow.UpdateMutebtnText();
            }
        }

        private void OnSongExited() {
            SetVisibility(true);
        }
        private void OnSongStarted() {
            SetVisibility(false);
        }
        private void OnGamePause() {
            SetVisibility(true);
        }
        private void OnGameResume() {
            SetVisibility(false);
        }

    }
}
