using HarmonyLib;
using UnityEngine;
using UnityEngine.EventSystems;

namespace AokanaAccess.Patches
{
    /// <summary>
    /// Harmony patches for UIConf class to add settings menu accessibility.
    /// </summary>
    [HarmonyPatch(typeof(UIConf))]
    public static class UIConfPatches
    {
        /// <summary>
        /// Patch for UIConf.ShowVisual method.
        /// Called when Visual settings tab is shown.
        /// </summary>
        [HarmonyPatch("ShowVisual")]
        [HarmonyPostfix]
        public static void ShowVisual_Postfix(UIConf __instance)
        {
            DebugLogger.Log("UIConf.ShowVisual called");

            // If menu is not active, this is the first time opening
            if (!SettingsHandler.IsActive())
            {
                SettingsHandler.OnMenuOpened();

                // Auto-focus first control for keyboard navigation
                try
                {
                    // Use grp_visual from __instance
                    GameObject visualGroup = __instance.grp_visual;
                    if (visualGroup != null)
                    {
                        var selectable = visualGroup.GetComponentInChildren<UnityEngine.UI.Selectable>();
                        if (selectable != null && EventSystem.current != null)
                        {
                            // Suppress the announcement for this auto-focus
                            MenuHandler.SuppressNextAnnouncement();
                            EventSystem.current.SetSelectedGameObject(selectable.gameObject);
                            DebugLogger.Log("Auto-focused first control in settings menu");
                        }
                    }
                }
                catch (System.Exception ex)
                {
                    DebugLogger.Log($"Error setting initial focus in settings: {ex.Message}");
                }
            }

            // Get tab name from the tab button's text component
            string tabName = GetTabButtonText("tabVisual");
            SettingsHandler.OnTabChanged(tabName, 0);
        }

        /// <summary>
        /// Patch for UIConf.ShowText method.
        /// Called when Text settings tab is shown.
        /// </summary>
        [HarmonyPatch("ShowText")]
        [HarmonyPostfix]
        public static void ShowText_Postfix(UIConf __instance)
        {
            DebugLogger.Log("UIConf.ShowText called");

            // Get tab name from the tab button's text component
            string tabName = GetTabButtonText("tabText");
            SettingsHandler.OnTabChanged(tabName, 1);

            // Auto-focus first control in Text tab for keyboard navigation
            AutoFocusTabContent(__instance.grp_text, "Text");
        }

        /// <summary>
        /// Patch for UIConf.ShowSound method.
        /// Called when Sound settings tab is shown.
        /// </summary>
        [HarmonyPatch("ShowSound")]
        [HarmonyPostfix]
        public static void ShowSound_Postfix(UIConf __instance)
        {
            DebugLogger.Log("UIConf.ShowSound called");

            // Get tab name from the tab button's text component
            string tabName = GetTabButtonText("tabSound");
            SettingsHandler.OnTabChanged(tabName, 2);

            // Auto-focus first control in Sound tab for keyboard navigation
            AutoFocusTabContent(__instance.grp_sound, "Sound");
        }

        /// <summary>
        /// Patch for UIConf.ShowVoice method.
        /// Called when Voice settings tab is shown.
        /// </summary>
        [HarmonyPatch("ShowVoice")]
        [HarmonyPostfix]
        public static void ShowVoice_Postfix(UIConf __instance)
        {
            DebugLogger.Log("UIConf.ShowVoice called");

            // Get tab name from the tab button's text component
            string tabName = GetTabButtonText("tabVoice");
            SettingsHandler.OnTabChanged(tabName, 3);

            // Auto-focus first control in Voice tab for keyboard navigation
            AutoFocusTabContent(__instance.grp_voice, "Voice");
        }

        /// <summary>
        /// Get text from a tab button by finding it in the UI hierarchy.
        /// </summary>
        private static string GetTabButtonText(string buttonName)
        {
            try
            {
                // Find the settings menu canvas
                GameObject sysMenus = GameObject.Find("SysMenus");
                if (sysMenus == null)
                {
                    DebugLogger.Log("SysMenus not found");
                    return buttonName; // Fallback to button name
                }

                // Find the tab button
                Transform buttonTransform = sysMenus.transform.Find(buttonName);
                if (buttonTransform == null)
                {
                    DebugLogger.Log($"Tab button {buttonName} not found");
                    return buttonName; // Fallback to button name
                }

                // Try to get text from TextMeshProUGUI component
                var tmpText = buttonTransform.GetComponentInChildren<TMPro.TextMeshProUGUI>(true);
                if (tmpText != null && !string.IsNullOrEmpty(tmpText.text))
                {
                    DebugLogger.Log($"Found tab text from UI: {tmpText.text}");
                    return tmpText.text;
                }

                // Try legacy Text component
                var legacyText = buttonTransform.GetComponentInChildren<UnityEngine.UI.Text>(true);
                if (legacyText != null && !string.IsNullOrEmpty(legacyText.text))
                {
                    DebugLogger.Log($"Found tab text from UI (legacy): {legacyText.text}");
                    return legacyText.text;
                }

                DebugLogger.Log($"No text component found for {buttonName}");
                return buttonName; // Fallback to button name
            }
            catch (System.Exception ex)
            {
                DebugLogger.Log($"Error getting tab button text: {ex.Message}");
                return buttonName; // Fallback to button name
            }
        }

        /// <summary>
        /// Helper method to auto-focus the first control in a tab content group.
        /// Only focuses on controls within the active tab, not tab buttons.
        /// </summary>
        private static void AutoFocusTabContent(GameObject tabGroup, string tabName)
        {
            try
            {
                if (tabGroup == null || !tabGroup.activeInHierarchy)
                {
                    DebugLogger.Log($"{tabName} tab group is null or inactive");
                    return;
                }

                // Find all selectables in the tab group
                var selectables = tabGroup.GetComponentsInChildren<UnityEngine.UI.Selectable>(false);

                // Filter out tab buttons (they usually have "tab" in their name)
                UnityEngine.UI.Selectable firstControl = null;
                foreach (var selectable in selectables)
                {
                    if (selectable.gameObject.activeInHierarchy &&
                        !selectable.gameObject.name.ToLower().Contains("tab"))
                    {
                        firstControl = selectable;
                        break;
                    }
                }

                if (firstControl != null && EventSystem.current != null)
                {
                    // Suppress the announcement for this auto-focus
                    MenuHandler.SuppressNextAnnouncement();
                    EventSystem.current.SetSelectedGameObject(firstControl.gameObject);
                    DebugLogger.Log($"Auto-focused first control in {tabName} tab: {firstControl.gameObject.name}");
                }
                else
                {
                    DebugLogger.Log($"No suitable control found in {tabName} tab for auto-focus");
                }
            }
            catch (System.Exception ex)
            {
                DebugLogger.Log($"Error setting focus in {tabName} tab: {ex.Message}");
            }
        }

        /// <summary>
        /// Patch for UIConf.OnDisable method.
        /// Called when settings menu is closed.
        /// </summary>
        [HarmonyPatch("OnDisable")]
        [HarmonyPrefix]
        public static void OnDisable_Prefix()
        {
            DebugLogger.Log("UIConf.OnDisable called - menu closing");
            SettingsHandler.OnMenuClosed();
            MenuHandler.AnnounceMenuClose(Loc.Get("menu_settings"));
        }
    }
}
