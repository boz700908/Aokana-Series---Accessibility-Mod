using HarmonyLib;
using UnityEngine;
using UnityEngine.EventSystems;

namespace AokanaAccess.Patches
{
    /// <summary>
    /// Harmony patches for UIConfirm class to add confirmation dialog accessibility.
    /// </summary>
    [HarmonyPatch(typeof(UIConfirm))]
    public static class UIConfirmPatches
    {
        /// <summary>
        /// Patch for UIConfirm.ShowPrompt method.
        /// Called when a confirmation dialog is displayed.
        /// </summary>
        [HarmonyPatch("ShowPrompt")]
        [HarmonyPostfix]
        public static void ShowPrompt_Postfix(UIConfirm __instance, string bgpath, bool skipPrompt)
        {
            // Always log this, not just in debug mode
            MelonLoader.MelonLogger.Msg($"[AokanaAccess] UIConfirm.ShowPrompt called with bgpath: {bgpath}, skipPrompt: {skipPrompt}");

            if (!skipPrompt)
            {
                MenuHandler.OnConfirmDialogOpened(bgpath);

                // Set initial focus to first button (0confirm = No)
                SetInitialFocus(__instance);
            }
            else
            {
                MelonLoader.MelonLogger.Msg("[AokanaAccess] Confirmation dialog skipped due to skipPrompt=true (user settings)");
            }
        }

        /// <summary>
        /// Set initial focus to the first button in the confirmation dialog.
        /// </summary>
        private static void SetInitialFocus(UIConfirm instance)
        {
            try
            {
                // Find the first button (0confirm)
                Transform button0 = instance.transform.Find("0confirm");
                if (button0 != null && EventSystem.current != null)
                {
                    DebugLogger.Log("Setting initial focus to 0confirm button");

                    // Suppress announcement for auto-focus (only announce on keyboard navigation)
                    MenuHandler.SuppressNextAnnouncement();

                    EventSystem.current.SetSelectedGameObject(button0.gameObject);
                }
                else
                {
                    DebugLogger.Log("Failed to find 0confirm button or EventSystem");
                }
            }
            catch (System.Exception ex)
            {
                DebugLogger.Log($"Error setting initial focus in confirmation dialog: {ex.Message}");
            }
        }

        /// <summary>
        /// Patch for UIConfirm.OnDisable method.
        /// Called when the confirmation dialog is closed.
        /// </summary>
        [HarmonyPatch("OnDisable")]
        [HarmonyPrefix]
        public static void OnDisable_Prefix()
        {
            DebugLogger.Log("UIConfirm.OnDisable called - dialog closing");
            MenuHandler.OnConfirmDialogClosed();
        }

        /// <summary>
        /// Patch for UIConfirm.LateUpdate method.
        /// Monitor focus changes for button navigation.
        /// </summary>
        [HarmonyPatch("LateUpdate")]
        [HarmonyPostfix]
        public static void LateUpdate_Postfix()
        {
            MenuHandler.CheckFocusChange();
        }
    }
}
