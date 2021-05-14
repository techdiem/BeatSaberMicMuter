using System.Runtime.CompilerServices;
using IPA.Config.Stores;
using UnityEngine;

[assembly: InternalsVisibleTo(GeneratedStore.AssemblyVisibilityTarget)]
namespace MicMuter.Configuration
{
    internal class PluginConfig
    {
        public static PluginConfig Instance { get; set; }
        public virtual string MicDeviceID { get; set; } = "";
        public virtual bool Enabled { get; set; } = true;
        public virtual bool UnmuteOnPause { get; set; } = true;
        public virtual bool MultiEnabled { get; set; } = true;

        //Floating mute button
        public virtual Vector3 screenPos { get; set; } = new Vector3(-1f, 0.6f, 2f);
        public virtual Quaternion screenRot { get; set; } = Quaternion.Euler(25f, 330f, 6.5f);

        /// <summary>
        /// This is called whenever BSIPA reads the config from disk (including when file changes are detected).
        /// </summary>
        public virtual void OnReload()
        {
        }

        /// <summary>
        /// Call this to force BSIPA to update the config file. This is also called by BSIPA if it detects the file was modified.
        /// </summary>
        public virtual void Changed()
        {
            //Update audio device
            MicDeviceUtils.SelectConfiguredMic(MicDeviceID);
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
