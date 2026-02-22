using HarmonyLib;

namespace AokanaAccess.Patches
{
    /// <summary>
    /// Harmony patches for UIAdv class to intercept dialog display.
    /// </summary>
    [HarmonyPatch(typeof(UIAdv))]
    public static class UIAdvPatches
    {
        /// <summary>
        /// Patch for UIAdv.ShowText method.
        /// This is called whenever new dialog text is displayed in the game.
        /// </summary>
        [HarmonyPatch("ShowText")]
        [HarmonyPostfix]
        public static void ShowText_Postfix(string txin, string dispnamein, bool updateonly)
        {
            // Only announce if this is new text, not an update
            if (updateonly)
            {
                return;
            }

            DebugLogger.Log($"=== ShowText Raw Input ===");
            DebugLogger.Log($"Raw Name: '{dispnamein}'");
            DebugLogger.Log($"Raw Text: '{txin}'");

            // Extract the correct language text
            // The game uses ASCII character 2 (␂) to separate languages
            string characterName = ExtractCurrentLanguageText(dispnamein);
            string dialogText = ExtractCurrentLanguageText(txin);

            DebugLogger.Log($"Extracted Name: '{characterName}'");
            DebugLogger.Log($"Extracted Text: '{dialogText}'");

            // Clean up any remaining formatting tags
            characterName = CleanText(characterName);
            dialogText = CleanText(dialogText);

            DebugLogger.Log($"Final Name: '{characterName}'");
            DebugLogger.Log($"Final Text: '{dialogText}'");

            // Notify the DialogHandler
            DialogHandler.OnDialogShown(characterName, dialogText);
        }

        /// <summary>
        /// Extract text for the current language.
        /// The game stores multi-language text separated by ASCII character 2.
        /// Format: text_lang0␂text_lang1␂text_lang2
        /// </summary>
        private static string ExtractCurrentLanguageText(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return string.Empty;
            }

            // Debug: Check what characters are in the string
            DebugLogger.Log($"Text length: {text.Length}");
            if (text.Length > 0 && text.Length < 200)
            {
                for (int i = 0; i < text.Length; i++)
                {
                    char c = text[i];
                    if (c < 32 || c == '␂')  // Control characters or the separator symbol
                    {
                        DebugLogger.Log($"  Char at {i}: ASCII {(int)c} (0x{((int)c):X2})");
                    }
                }
            }

            // Try multiple possible separators
            string[] parts = null;

            // Try ASCII 2 (STX)
            if (text.Contains("\u0002"))
            {
                parts = text.Split('\u0002');
                DebugLogger.Log($"Split by \\u0002: {parts.Length} parts");
            }
            // Try the visible separator character ␂ (U+2402)
            else if (text.Contains("␂"))
            {
                parts = text.Split('␂');
                DebugLogger.Log($"Split by ␂ (U+2402): {parts.Length} parts");
            }
            else
            {
                // No separator found, return as-is
                DebugLogger.Log("No separator found, returning as-is");
                return text;
            }

            for (int i = 0; i < parts.Length && i < 10; i++)
            {
                DebugLogger.Log($"  Part {i}: '{parts[i]}'");
            }

            // If there's only one part, return it (no multi-language)
            if (parts.Length <= 1)
            {
                DebugLogger.Log("Only one part after split");
                return parts[0];
            }

            // Get current language index from EngineMain
            // 0 = Japanese, 1 = English, 2 = Chinese (typically)
            int langIndex = (int)EngineMain.lang;
            DebugLogger.Log($"Current language index: {langIndex}");

            // Return the text for the current language
            if (langIndex >= 0 && langIndex < parts.Length)
            {
                DebugLogger.Log($"Returning part {langIndex}: '{parts[langIndex]}'");
                return parts[langIndex];
            }

            // Fallback to first language if index is out of range
            DebugLogger.Log($"Index out of range, returning part 0");
            return parts[0];
        }

        /// <summary>
        /// Clean text by removing TextMeshPro formatting tags.
        /// </summary>
        private static string CleanText(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return string.Empty;
            }

            // Remove TextMeshPro tags like <color=#ffffff>, <b>, <i>, etc.
            string cleaned = text;

            // Remove <color=...> tags
            while (cleaned.Contains("<color="))
            {
                int start = cleaned.IndexOf("<color=");
                int end = cleaned.IndexOf(">", start);
                if (end > start)
                {
                    cleaned = cleaned.Remove(start, end - start + 1);
                }
                else
                {
                    break;
                }
            }

            // Remove closing tags and simple tags
            cleaned = cleaned.Replace("</color>", "");
            cleaned = cleaned.Replace("<b>", "");
            cleaned = cleaned.Replace("</b>", "");
            cleaned = cleaned.Replace("<i>", "");
            cleaned = cleaned.Replace("</i>", "");

            return cleaned.Trim();
        }
    }
}
