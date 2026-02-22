using HarmonyLib;
using UnityEngine;

namespace AokanaAccess.Patches
{
    /// <summary>
    /// Harmony patches for UIFlow class to detect menu openings.
    /// </summary>
    [HarmonyPatch(typeof(UIFlow))]
    public static class UIFlowPatches
    {
        /// <summary>
        /// Patch for UIFlow.CallScreen method.
        /// Detect when voice bookmarks menu is opened.
        /// </summary>
        [HarmonyPatch("CallScreen")]
        [HarmonyPostfix]
        public static void CallScreen_Postfix(GameObject uio)
        {
            if (uio == null)
            {
                return;
            }

            DebugLogger.Log($"UIFlow.CallScreen called with: {uio.name}");

            // Check if this is the voice bookmarks menu
            if (uio.name == "VBookmark" || uio.name.Contains("VBookmark") || uio.name.Contains("Vbookmark"))
            {
                DebugLogger.Log("Voice bookmarks menu detected via CallScreen");
                string announcement = Loc.Get("menu_voice_bookmarks");
                ScreenReader.Speak(announcement, interrupt: true);
                MenuHandler.OnMenuOpened(announcement);
            }
        }
    }
}
