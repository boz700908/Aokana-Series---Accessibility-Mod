using HarmonyLib;
using UnityEngine;
using UnityEngine.EventSystems;

namespace AokanaAccess.Patches
{
    /// <summary>
    /// Harmony patches for UIVBookmark class to add voice bookmarks menu accessibility.
    /// </summary>
    [HarmonyPatch(typeof(UIVBookmark))]
    public static class UIVBookmarkPatches
    {
        /// <summary>
        /// Patch for UIVBookmark.RefreshUI method.
        /// Set initial focus after menu refresh.
        /// </summary>
        [HarmonyPatch("RefreshUI", MethodType.Normal)]
        [HarmonyPostfix]
        public static void RefreshUI_Postfix(UIVBookmark __instance)
        {
            // The game clears focus in RefreshUI, so we need to set it back
            // Find the first item button and set focus to it
            try
            {
                Transform bookmarksTransform = __instance.transform.Find("bookmarks");
                if (bookmarksTransform != null)
                {
                    Transform item0 = bookmarksTransform.Find("item0");
                    if (item0 != null && EventSystem.current != null)
                    {
                        DebugLogger.Log("Setting initial focus to item0 in voice bookmarks menu");

                        // Suppress announcement for auto-focus (only announce on keyboard navigation)
                        MenuHandler.SuppressNextAnnouncement();

                        EventSystem.current.SetSelectedGameObject(item0.gameObject);
                    }
                }
            }
            catch (System.Exception ex)
            {
                DebugLogger.Log($"Error setting initial focus in voice bookmarks: {ex.Message}");
            }
        }

        /// <summary>
        /// Patch for UIVBookmark.OnDisable method.
        /// Called when the voice bookmarks menu is closed.
        /// </summary>
        [HarmonyPatch("OnDisable")]
        [HarmonyPrefix]
        public static void OnDisable_Prefix()
        {
            DebugLogger.Log("UIVBookmark.OnDisable called - submenu closing");
            // Announce menu close but preserve parent menu state
            MenuHandler.AnnounceMenuClose(Loc.Get("menu_voice_bookmarks"));
        }
    }
}
