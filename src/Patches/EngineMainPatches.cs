using HarmonyLib;

namespace AokanaAccess.Patches
{
    /// <summary>
    /// Harmony patches for EngineMain class to initialize quick access features.
    /// </summary>
    [HarmonyPatch(typeof(EngineMain))]
    public static class EngineMainPatches
    {
        /// <summary>
        /// Patch for EngineMain.Start method.
        /// Initialize handlers with engine reference.
        /// </summary>
        [HarmonyPatch("Start")]
        [HarmonyPostfix]
        public static void Start_Postfix(EngineMain __instance)
        {
            DebugLogger.Log("EngineMain.Start called, initializing handlers");
            QuickAccessHandler.Initialize(__instance);
            SettingsHandler.Initialize(__instance);
            SlotSaveHandler.Initialize(__instance);

            // Update localization after game language is loaded
            Loc.UpdateLanguage();
        }

        /// <summary>
        /// Patch for EngineMain.SetLang method.
        /// Update mod language when game language changes.
        /// </summary>
        [HarmonyPatch("SetLang")]
        [HarmonyPostfix]
        public static void SetLang_Postfix(EngineMain.Language newlang)
        {
            DebugLogger.Log($"EngineMain.SetLang called with: {newlang}");
            Loc.UpdateLanguage();
        }

        /// <summary>
        /// Patch for EngineMain.QuitHandler method.
        /// Log when Alt+F4 is pressed.
        /// </summary>
        [HarmonyPatch("QuitHandler", MethodType.Normal)]
        [HarmonyPrefix]
        public static void QuitHandler_Prefix(EngineMain __instance)
        {
            // Always log this, not just in debug mode
            MelonLoader.MelonLogger.Msg($"[AokanaAccess] QuitHandler called - allowQuit: {__instance.allowQuit}, mode: {__instance.uiflow.mode}");
        }

        /// <summary>
        /// Patch for EngineMain.QuitGame method.
        /// Shutdown ScreenReader when actually quitting.
        /// </summary>
        [HarmonyPatch("QuitGame")]
        [HarmonyPrefix]
        public static void QuitGame_Prefix()
        {
            MelonLoader.MelonLogger.Msg("[AokanaAccess] QuitGame called - shutting down ScreenReader");
            ScreenReader.Shutdown();
        }
    }
}
