using System.Collections.Generic;
using HarmonyLib;

namespace AokanaAccess.Patches
{
    /// <summary>
    /// Harmony patches for AdvChoice class to intercept choice menus.
    /// </summary>
    [HarmonyPatch(typeof(AdvChoice))]
    public static class AdvChoicePatches
    {
        /// <summary>
        /// Patch for AdvChoice.ShowMenu method.
        /// This is called when a choice menu is displayed.
        /// </summary>
        [HarmonyPatch("ShowMenu")]
        [HarmonyPostfix]
        public static void ShowMenu_Postfix(List<string>[] mplist, string id)
        {
            // Extract choices for current language
            List<string> choices = (mplist.Length <= 1) ? mplist[0] : mplist[(int)EngineMain.lang];

            // Extract text from each choice (they may contain language separators)
            List<string> cleanedChoices = new List<string>();
            foreach (string choice in choices)
            {
                string cleaned = ExtractCurrentLanguageText(choice);
                cleanedChoices.Add(cleaned);
            }

            DebugLogger.Log($"ShowMenu called with {cleanedChoices.Count} choices");
            for (int i = 0; i < cleanedChoices.Count; i++)
            {
                DebugLogger.Log($"  Choice {i + 1}: '{cleanedChoices[i]}'");
            }

            // Notify the ChoiceHandler
            ChoiceHandler.OnChoicesShown(cleanedChoices);
        }

        /// <summary>
        /// Patch for AdvChoice.HideMenu method.
        /// This is called when the choice menu is hidden.
        /// </summary>
        [HarmonyPatch("HideMenu")]
        [HarmonyPostfix]
        public static void HideMenu_Postfix()
        {
            DebugLogger.Log("HideMenu called");
            ChoiceHandler.OnChoicesHidden();
        }

        /// <summary>
        /// Extract text for the current language (same logic as UIAdvPatches).
        /// </summary>
        private static string ExtractCurrentLanguageText(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return string.Empty;
            }

            // Try the visible separator character ␂ (U+2402)
            if (text.Contains("␂"))
            {
                string[] parts = text.Split('␂');
                int langIndex = (int)EngineMain.lang;

                if (langIndex >= 0 && langIndex < parts.Length)
                {
                    return parts[langIndex].Trim();
                }

                return parts[0].Trim();
            }

            return text.Trim();
        }
    }
}
