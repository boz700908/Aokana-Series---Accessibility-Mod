using HarmonyLib;

namespace AokanaAccess.Patches
{
    /// <summary>
    /// Harmony patches for UIBacklog class to intercept backlog display and navigation.
    /// </summary>
    [HarmonyPatch(typeof(UIBacklog))]
    public static class UIBacklogPatches
    {
        /// <summary>
        /// Patch for UIBacklog.ShowBacklog method.
        /// This is called when the backlog is opened.
        /// </summary>
        [HarmonyPatch("ShowBacklog")]
        [HarmonyPostfix]
        public static void ShowBacklog_Postfix(UIBacklog __instance)
        {
            // Access the private field 'b' using Harmony's Traverse
            Backlog backlog = Traverse.Create(__instance).Field("b").GetValue<Backlog>();

            if (backlog != null)
            {
                DebugLogger.Log("ShowBacklog called");
                BacklogHandler.OnBacklogShown(backlog);
            }
        }

        /// <summary>
        /// Patch for UIBacklog.HideBacklog method.
        /// This is called when the backlog is closed.
        /// </summary>
        [HarmonyPatch("HideBacklog", MethodType.Normal)]
        [HarmonyPrefix]
        public static void HideBacklog_Prefix()
        {
            DebugLogger.Log("HideBacklog called");
            BacklogHandler.OnBacklogHidden();
        }

        /// <summary>
        /// Patch for UIBacklog.OnDisable method.
        /// This is also called when the backlog is closed (Unity lifecycle).
        /// </summary>
        [HarmonyPatch("OnDisable")]
        [HarmonyPrefix]
        public static void OnDisable_Prefix()
        {
            DebugLogger.Log("OnDisable called");
            BacklogHandler.OnBacklogHidden();
        }

        /// <summary>
        /// Patch for UIBacklog.RefreshUI method.
        /// This is called when the backlog scrolls to a new position.
        /// </summary>
        [HarmonyPatch("RefreshUI")]
        [HarmonyPostfix]
        public static void RefreshUI_Postfix(UIBacklog __instance)
        {
            // Access the private field 'idxBegin' using Harmony's Traverse
            int idxBegin = Traverse.Create(__instance).Field("idxBegin").GetValue<int>();

            DebugLogger.Log($"RefreshUI called with idxBegin={idxBegin}");
            BacklogHandler.OnBacklogScrolled(idxBegin);
        }
    }
}
