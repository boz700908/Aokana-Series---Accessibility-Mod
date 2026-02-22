using UnityEngine;
using TMPro;

namespace AokanaAccess
{
    public static class SubtitleDebug
    {
        public static void FindAllTextComponents()
        {
            MelonLoader.MelonLogger.Msg("[SubtitleDebug] Searching for all TextMeshProUGUI components...");
            
            var allObjects = GameObject.FindObjectsOfType<TextMeshProUGUI>();
            MelonLoader.MelonLogger.Msg($"[SubtitleDebug] Found {allObjects.Length} TextMeshProUGUI components");
            
            foreach (var tmp in allObjects)
            {
                if (tmp.gameObject.activeInHierarchy && !string.IsNullOrEmpty(tmp.text))
                {
                    MelonLoader.MelonLogger.Msg($"[SubtitleDebug] Active: {tmp.gameObject.name} - Text: '{tmp.text}'");
                }
            }
        }
    }
}
