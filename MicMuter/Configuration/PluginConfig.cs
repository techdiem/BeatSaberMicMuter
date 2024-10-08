﻿using System.Runtime.CompilerServices;
using IPA.Config.Stores;
using UnityEngine;
using MicMuter.UI;

[assembly: InternalsVisibleTo(GeneratedStore.AssemblyVisibilityTarget)]
namespace MicMuter.Configuration
{
    internal class PluginConfig
    {
        public static PluginConfig Instance { get; set; }
        public virtual string MicDeviceID { get; set; } = "";

        //Automatic mute
        public virtual bool SingleEnabled { get; set; } = true;
        public virtual bool UnmuteOnPause { get; set; } = true;
        public virtual bool MultiEnabled { get; set; } = true;

        //Floating mute button
        public virtual bool ScreenEnabled { get; set; } = false;
        public virtual Vector3 ScreenPos { get; set; } = new Vector3(-1f, 0.6f, 2f);
        public virtual Quaternion ScreenRot { get; set; } = Quaternion.Euler(25f, 330f, 6.5f);

        //Push-to-talk
        public virtual bool PTTEnabled { get; set; } = false;
        public virtual string PTTKey { get; set; } = "L+R Trigger";
        public virtual string PTTActionMode { get; set; } = "Push-to-talk";

        /// <summary>
        /// This is called whenever BSIPA reads the config from disk (including when file changes are detected).
        /// </summary>
        public virtual void OnReload() {
        }

        /// <summary>
        /// Call this to force BSIPA to update the config file. This is also called by BSIPA if it detects the file was modified.
        /// </summary>
        public virtual void Changed()
        {
            if (MicDeviceID != "")
            {
                //Update audio device
                MicDeviceUtils.SelectConfiguredMic(MicDeviceID);
                //Mute when push-to-talk mode is used
                if (PTTEnabled)
                {
                    MicDeviceUtils.SetMicMute(PTTActionMode == "Push-to-talk");
                }

                //Toggle mute button screen
                if (Plugin.startupReady)
                {
                    if (ScreenEnabled)
                    {
                        MuteButtonWindowController.Instance.ShowMuteWindow();
                    }
                    else if (MuteButtonWindowController.Instance.MuteButtonScreen != null)
                    {
                        MuteButtonWindowController.Instance.Cleanup();
                    }
                }
            }
            else
            {
                Plugin.Log.Warn("Selected Mic ID is empty, is this your first start or did the device ID change?");
            }
        }

        /// <summary>
        /// Call this to have BSIPA copy the values from <paramref name="other"/> into this config.
        /// </summary>
        public virtual void CopyFrom(PluginConfig other)
        {
            // This instance's members populated from other
        }
    }
}
