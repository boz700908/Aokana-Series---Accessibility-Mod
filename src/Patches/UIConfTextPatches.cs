using HarmonyLib;

namespace AokanaAccess.Patches
{
    /// <summary>
    /// Harmony patches for UIConfText class to announce slider value changes.
    /// </summary>
    [HarmonyPatch(typeof(UIConfText))]
    public static class UIConfTextPatches
    {
        /// <summary>
        /// Patch for ChangeWinOpacity method.
        /// Announces window opacity changes.
        /// </summary>
        [HarmonyPatch("ChangeWinOpacity")]
        [HarmonyPostfix]
        public static void ChangeWinOpacity_Postfix(float v, UIConfText __instance)
        {
            // Only announce if the Text settings tab is actually visible
            // Check if the parent grp_text GameObject is active
            if (!__instance.gameObject.activeInHierarchy)
                return;

            // Additional check: find grp_text parent and verify it's active
            UnityEngine.Transform parent = __instance.transform.parent;
            while (parent != null)
            {
                if (parent.name == "grp_text")
                {
                    if (!parent.gameObject.activeInHierarchy)
                    {
                        DebugLogger.Log("Window opacity change ignored - grp_text is not active");
                        return;
                    }
                    break;
                }
                parent = parent.parent;
            }

            int percentage = (int)v;
            string announcement = $"{Loc.Get("window_opacity")}: {percentage}%";
            ScreenReader.Speak(announcement, interrupt: true);
            DebugLogger.Log($"Window opacity changed to: {percentage}%");
        }
    }
}
