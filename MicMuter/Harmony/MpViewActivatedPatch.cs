using HarmonyLib;

namespace MicMuter.Harmony {
    [HarmonyPatch(typeof(MultiplayerModeSelectionViewController), "DidActivate", MethodType.Normal)]
    class MpViewActivatedPatch {
        public static void Postfix(MultiplayerModeSelectionViewController __instance, bool firstActivation, bool addedToHierarchy, bool screenSystemEnabling) {
            
            //Set up multiplayer session manager
            EventMute.SetupMP();
        }
    }
}
