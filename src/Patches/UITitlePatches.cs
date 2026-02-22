using HarmonyLib;

namespace AokanaAccess.Patches
{
    /// <summary>
    /// Harmony patches for UITitle class to add main menu accessibility.
    /// </summary>
    [HarmonyPatch(typeof(UITitle))]
    public static class UITitlePatches
    {
        /// <summary>
        /// Patch for UITitle.ShowTitleMenu method.
        /// Called when the main menu is displayed.
        /// </summary>
        [HarmonyPatch("ShowTitleMenu")]
        [HarmonyPostfix]
        public static void ShowTitleMenu_Postfix()
        {
            DebugLogger.Log("UITitle.ShowTitleMenu called");
            MenuHandler.OnMenuOpened(Loc.Get("menu_main"));
        }

        /// <summary>
        /// Patch for UITitle.ShowExtraMenu method.
        /// Called when the extra menu is displayed.
        /// </summary>
        [HarmonyPatch("ShowExtraMenu", MethodType.Normal)]
        [HarmonyPostfix]
        public static void ShowExtraMenu_Postfix()
        {
            DebugLogger.Log("UITitle.ShowExtraMenu called");
            MenuHandler.OnMenuOpened(Loc.Get("menu_extra"));
        }

        /// <summary>
        /// Patch for UITitle.CloseExtraMenu method.
        /// Called when the extra menu is closed.
        /// </summary>
        [HarmonyPatch("CloseExtraMenu", MethodType.Normal)]
        [HarmonyPrefix]
        public static void CloseExtraMenu_Prefix()
        {
            DebugLogger.Log("UITitle.CloseExtraMenu called");
            MenuHandler.AnnounceMenuClose(Loc.Get("menu_extra"));
        }
    }
}
