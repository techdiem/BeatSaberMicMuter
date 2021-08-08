using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR;


//Credits: adapted from https://github.com/Zingabopp/BeatSaberMultiplayer/blob/master/BeatSaberMultiplayer/Misc/ControllersHelper.cs

namespace MicMuter {

    public delegate void ControllerButtonDelegate(bool state);
    static class ControllersHelper {
        private static bool initialized;
        private static InputDevice leftController;
        private static InputDevice rightController;
        private static int leftFailedTries = 0;
        private static int rightFailedTries = 0;
        private static readonly float TriggerThreshold = 0.85f;
        private static readonly float GripThreshold = 0.85f;

        public static bool LeftTriggerState;
        public static bool RightTriggerState;
        public static bool LeftGripState;
        public static bool RightGripState;

        public static event ControllerButtonDelegate LeftTriggerChanged;
        public static event ControllerButtonDelegate RightTriggerChanged;
        public static event ControllerButtonDelegate LeftGripChanged;
        public static event ControllerButtonDelegate RightGripChanged;

        internal static InputDevice LeftController {
            get {
                if (leftController.isValid) {
                    leftFailedTries = 0;
                    return leftController;
                }
                leftFailedTries++;
                if (leftFailedTries > 100) {
                    leftController = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
                    leftFailedTries = 0;
                }
                return leftController;
            }
            set => leftController = value;
        }
        internal static InputDevice RightController {
            get {
                if (rightController.isValid) {
                    rightFailedTries = 0;
                    return rightController;
                }
                rightFailedTries++;
                if (rightFailedTries > 100) {
                    rightController = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
                    rightFailedTries = 0;
                }
                return rightController;
            }
            set => rightController = value;
        }

        public static void Init() {
            if (initialized)
                return;
            InputDevices.deviceConnected += OnDeviceConnected;
            InputDevices.deviceDisconnected += OnDeviceDisconnected;

            initialized = true;
        }

        private static void OnDeviceDisconnected(InputDevice device) {
            if (device.characteristics.HasFlag(InputDeviceCharacteristics.Right)) {
                Plugin.Log.Info("Right controller disconnected.");
            }
            else if (device.characteristics.HasFlag(InputDeviceCharacteristics.Left)) {
                Plugin.Log.Info("Left controller disconnected.");
            }
        }

        private static void OnDeviceConnected(InputDevice device) {

            if (device.characteristics.HasFlag(InputDeviceCharacteristics.Right)) {
                RightController = device;
                Plugin.Log.Info("Right controller connected.");
            }
            else if (device.characteristics.HasFlag(InputDeviceCharacteristics.Left)) {
                LeftController = device;
                Plugin.Log.Info("Left controller connected.");
            }
        }

        public static void UpdateEvents() {
            //Left
            InputDevice device = LeftController;
            if (device.isValid) {
                if (device.TryGetFeatureValue(CommonUsages.trigger, out float ltvalue)) {
                    bool state = ltvalue > TriggerThreshold;
                    if (state != LeftTriggerState) {
                        LeftTriggerState = state;
                        LeftTriggerChanged(state);
                    }
                }
                if (device.TryGetFeatureValue(CommonUsages.grip, out float lgvalue)) {
                    bool state = lgvalue > GripThreshold;
                    if (state != LeftGripState) {
                        LeftGripState = state;
                        LeftGripChanged(state);
                    }
                }
            }

            //Right
            device = RightController;
            if (device.isValid) {
                if (device.TryGetFeatureValue(CommonUsages.trigger, out float rtvalue)) {
                    bool state = rtvalue > TriggerThreshold;
                    if (state != RightTriggerState) {
                        RightTriggerState = state;
                        RightTriggerChanged(state);
                    }
                }
                if (device.TryGetFeatureValue(CommonUsages.grip, out float rgvalue)) {
                    bool state = rgvalue > GripThreshold;
                    if (state != RightGripState) {
                        RightGripState = state;
                        RightGripChanged(state);
                    }
                }
            }
        }

    }
}
