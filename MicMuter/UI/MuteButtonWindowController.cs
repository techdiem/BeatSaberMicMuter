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
            BSEvents.menuSceneActive += OnSongExited;
            BSEvents.gameSceneActive += OnSongStarted;
            BSEvents.songPaused += OnGamePause;
            BSEvents.songUnpaused += OnGameResume;
        }

        private void BSEvents_earlyMenuSceneLoadedFresh(ScenesTransitionSetupDataSO obj) {
            if (MuteButtonScreen != null) {
                GameObject.Destroy(MuteButtonScreen.gameObject);
                MuteButtonScreen = null;
            }
        }

        public void ShowMuteWindow() {
            if (MuteButtonScreen == null) {
                MuteButtonScreen = CreateFloatingScreen();
                MuteButtonWindow = BeatSaberUI.CreateViewController<MuteButtonWindow>();
                MuteButtonWindow.ParentCoordinator = this;
                MuteButtonScreen.SetRootViewController(MuteButtonWindow, HMUI.ViewController.AnimationType.None);
            }
            MuteButtonScreen.gameObject.SetActive(true);
            MuteButtonWindow.UpdateMutebtnText();
        }

        public FloatingScreen CreateFloatingScreen() {
            FloatingScreen screen = FloatingScreen.CreateFloatingScreen(
                new Vector2(25, 15), false,
                PluginConfig.Instance.screenPos,
                PluginConfig.Instance.screenRot);

            screen.HandleReleased -= OnRelease;
            screen.HandleReleased += OnRelease;

            GameObject.DontDestroyOnLoad(screen.gameObject);
            return screen;
        }

        private void OnRelease(object _, FloatingScreenHandleEventArgs posRot) {
            Vector3 newPos = posRot.Position;
            Quaternion newRot = posRot.Rotation;

            PluginConfig.Instance.screenPos = newPos;
            PluginConfig.Instance.screenRot = newRot;
        }

        private void OnSongExited() {
            MuteButtonScreen.gameObject.SetActive(true);
            MuteButtonWindow.UpdateMutebtnText();
        }
        private void OnSongStarted() {
            MuteButtonScreen.gameObject.SetActive(false);
        }
        private void OnGamePause() {
            MuteButtonScreen.gameObject.SetActive(true);
            MuteButtonWindow.UpdateMutebtnText();
        }
        private void OnGameResume() {
            MuteButtonScreen.gameObject.SetActive(false);
        }

    }
}
