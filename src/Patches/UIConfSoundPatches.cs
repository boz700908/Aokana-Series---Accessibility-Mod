using HarmonyLib;

namespace AokanaAccess.Patches
{
    /// <summary>
    /// Harmony patches for UIConfSound class to announce slider value changes.
    /// </summary>
    [HarmonyPatch(typeof(UIConfSound))]
    public static class UIConfSoundPatches
    {
        /// <summary>
        /// Patch for ChangeVolMaster method.
        /// Announces master volume changes.
        /// </summary>
        [HarmonyPatch("ChangeVolMaster")]
        [HarmonyPostfix]
        public static void ChangeVolMaster_Postfix(float v, UIConfSound __instance)
        {
            // Only announce if the settings menu is actually visible
            if (!__instance.gameObject.activeInHierarchy)
                return;

            int percentage = (int)v;
            string announcement = $"{Loc.Get("vol_master")}: {percentage}%";
            ScreenReader.Speak(announcement, interrupt: true);
            DebugLogger.Log($"Master volume changed to: {percentage}%");
        }

        /// <summary>
        /// Patch for ChangeVolBgm method.
        /// Announces BGM volume changes.
        /// </summary>
        [HarmonyPatch("ChangeVolBgm")]
        [HarmonyPostfix]
        public static void ChangeVolBgm_Postfix(float v, UIConfSound __instance)
        {
            // Only announce if the settings menu is actually visible
            if (!__instance.gameObject.activeInHierarchy)
                return;

            int percentage = (int)v;
            string announcement = $"{Loc.Get("vol_bgm")}: {percentage}%";
            ScreenReader.Speak(announcement, interrupt: true);
            DebugLogger.Log($"BGM volume changed to: {percentage}%");
        }

        /// <summary>
        /// Patch for ChangeVolSfx method.
        /// Announces sound effects volume changes.
        /// </summary>
        [HarmonyPatch("ChangeVolSfx")]
        [HarmonyPostfix]
        public static void ChangeVolSfx_Postfix(float v, UIConfSound __instance)
        {
            // Only announce if the settings menu is actually visible
            if (!__instance.gameObject.activeInHierarchy)
                return;

            int percentage = (int)v;
            string announcement = $"{Loc.Get("vol_sfx")}: {percentage}%";
            ScreenReader.Speak(announcement, interrupt: true);
            DebugLogger.Log($"Sound effects volume changed to: {percentage}%");
        }

        /// <summary>
        /// Patch for ChangeVolMovie method.
        /// Announces movie volume changes.
        /// </summary>
        [HarmonyPatch("ChangeVolMovie")]
        [HarmonyPostfix]
        public static void ChangeVolMovie_Postfix(float v, UIConfSound __instance)
        {
            // Only announce if the settings menu is actually visible
            if (!__instance.gameObject.activeInHierarchy)
                return;

            int percentage = (int)v;
            string announcement = $"{Loc.Get("vol_movie")}: {percentage}%";
            ScreenReader.Speak(announcement, interrupt: true);
            DebugLogger.Log($"Movie volume changed to: {percentage}%");
        }

        /// <summary>
        /// Patch for ChangeVolVoice method.
        /// Announces voice volume changes.
        /// </summary>
        [HarmonyPatch("ChangeVolVoice")]
        [HarmonyPostfix]
        public static void ChangeVolVoice_Postfix(float v, UIConfSound __instance)
        {
            // Only announce if the settings menu is actually visible
            if (!__instance.gameObject.activeInHierarchy)
                return;

            int percentage = (int)v;
            string announcement = $"{Loc.Get("vol_voice")}: {percentage}%";
            ScreenReader.Speak(announcement, interrupt: true);
            DebugLogger.Log($"Voice volume changed to: {percentage}%");
        }
    }
}
