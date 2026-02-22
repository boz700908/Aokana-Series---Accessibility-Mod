using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace AokanaAccess
{
    /// <summary>
    /// Debug exporter for menu and dialog IDs with images.
    /// Activated with Alt+Shift+E in debug mode.
    /// </summary>
    public static class DebugExporter
    {
        private static bool _isExporting = false;

        /// <summary>
        /// Export all menus and dialogs with their IDs and images.
        /// Only exports the current language since UI doesn't reload when switching EngineMain.lang.
        /// </summary>
        public static void ExportAllMenusAndDialogs()
        {
            if (_isExporting)
            {
                MelonLoader.MelonLogger.Warning("[DebugExporter] Export already in progress");
                return;
            }

            _isExporting = true;

            try
            {
                MelonLoader.MelonLogger.Msg("[DebugExporter] Starting export...");

                // Get game directory
                string gameDirectory = Directory.GetCurrentDirectory();
                string debugPath = Path.Combine(gameDirectory, "debug");

                // Create debug directory
                if (!Directory.Exists(debugPath))
                {
                    Directory.CreateDirectory(debugPath);
                }

                // Get current language
                string currentLang = GetCurrentLanguage();
                MelonLoader.MelonLogger.Msg($"[DebugExporter] Current game language: {currentLang}");

                // Export only current language (UI doesn't reload when switching EngineMain.lang)
                ExportForLanguage(currentLang, debugPath);

                MelonLoader.MelonLogger.Msg($"[DebugExporter] Export completed! Check {debugPath}/{currentLang}");
                ScreenReader.Speak($"Export completed for {currentLang}", interrupt: true);
            }
            catch (Exception ex)
            {
                MelonLoader.MelonLogger.Error($"[DebugExporter] Export error: {ex.Message}");
                MelonLoader.MelonLogger.Error($"[DebugExporter] Stack trace: {ex.StackTrace}");
                ScreenReader.Speak("Export failed", interrupt: true);
            }
            finally
            {
                _isExporting = false;
            }
        }

        /// <summary>
        /// Export for the current language.
        /// </summary>
        private static void ExportForLanguage(string language, string debugPath)
        {
            try
            {
                MelonLoader.MelonLogger.Msg($"[DebugExporter] Exporting for language: {language}");

                // Create language directory
                string langPath = Path.Combine(debugPath, language);
                if (!Directory.Exists(langPath))
                {
                    Directory.CreateDirectory(langPath);
                }

                // Collect current scene state
                List<MenuData> menus = CollectMenuData();
                List<DialogData> dialogs = CollectDialogData();

                MelonLoader.MelonLogger.Msg($"[DebugExporter] Collected {menus.Count} menus and {dialogs.Count} dialogs");

                // Export to JSON
                ExportToJson(menus, dialogs, langPath, language);

                // Export images
                ExportImages(menus, dialogs, langPath);

                MelonLoader.MelonLogger.Msg($"[DebugExporter] Completed export for {language}");
            }
            catch (Exception ex)
            {
                MelonLoader.MelonLogger.Error($"[DebugExporter] Error exporting {language}: {ex.Message}");
                MelonLoader.MelonLogger.Error($"[DebugExporter] Stack trace: {ex.StackTrace}");
            }
        }

        /// <summary>
        /// Collect all menu data from the scene.
        /// </summary>
        private static List<MenuData> CollectMenuData()
        {
            List<MenuData> menus = new List<MenuData>();

            try
            {
                MelonLoader.MelonLogger.Msg("[DebugExporter] Starting menu collection...");

                // Find specific known UI objects based on game code
                // UITitle - Title screen menu
                GameObject titleBtnGrp = GameObject.Find("TitleBtnGrp");
                if (titleBtnGrp != null)
                {
                    MenuData titleMenu = new MenuData
                    {
                        id = "TitleBtnGrp",
                        name = "Title Menu",
                        hierarchy = GetHierarchyPath(titleBtnGrp.transform),
                        buttons = CollectButtons(titleBtnGrp)
                    };
                    menus.Add(titleMenu);
                    MelonLoader.MelonLogger.Msg($"[DebugExporter] Found TitleBtnGrp with {titleMenu.buttons.Count} buttons");
                }

                // SysMenus - System menus (settings, etc.)
                GameObject sysMenus = GameObject.Find("SysMenus");
                if (sysMenus != null)
                {
                    MenuData sysMenu = new MenuData
                    {
                        id = "SysMenus",
                        name = "System Menus",
                        hierarchy = GetHierarchyPath(sysMenus.transform),
                        buttons = CollectButtons(sysMenus)
                    };
                    menus.Add(sysMenu);
                    MelonLoader.MelonLogger.Msg($"[DebugExporter] Found SysMenus with {sysMenu.buttons.Count} buttons");
                }

                // UIConf - Settings menu (temporarily activate all tabs to collect buttons)
                GameObject uiConf = GameObject.Find("UIConf");
                if (uiConf != null)
                {
                    MelonLoader.MelonLogger.Msg("[DebugExporter] Found UIConf (Settings Menu)");

                    // Collect main UIConf buttons
                    MenuData confMenu = new MenuData
                    {
                        id = "UIConf",
                        name = "Settings Menu",
                        hierarchy = GetHierarchyPath(uiConf.transform),
                        buttons = CollectButtons(uiConf)
                    };
                    menus.Add(confMenu);
                    MelonLoader.MelonLogger.Msg($"[DebugExporter] UIConf main menu: {confMenu.buttons.Count} buttons");

                    // Collect buttons from each settings tab
                    string[] settingsTabs = new string[] { "grp_visual", "grp_text", "grp_sound", "grp_voice" };
                    foreach (string tabName in settingsTabs)
                    {
                        Transform tabTransform = uiConf.transform.Find(tabName);
                        if (tabTransform != null)
                        {
                            GameObject tabObj = tabTransform.gameObject;
                            bool wasActive = tabObj.activeSelf;

                            // Temporarily activate to collect buttons
                            tabObj.SetActive(true);

                            MenuData tabMenu = new MenuData
                            {
                                id = tabName,
                                name = $"Settings - {tabName}",
                                hierarchy = GetHierarchyPath(tabObj.transform),
                                buttons = CollectButtons(tabObj)
                            };
                            menus.Add(tabMenu);
                            MelonLoader.MelonLogger.Msg($"[DebugExporter] Found {tabName} with {tabMenu.buttons.Count} buttons");

                            // Restore original state
                            tabObj.SetActive(wasActive);
                        }
                    }
                }

                // Find all active canvases
                Canvas[] canvases = GameObject.FindObjectsOfType<Canvas>();
                MelonLoader.MelonLogger.Msg($"[DebugExporter] Found {canvases.Length} canvases");

                foreach (Canvas canvas in canvases)
                {
                    if (canvas.gameObject.activeInHierarchy)
                    {
                        // Skip if already collected
                        string canvasName = canvas.gameObject.name;
                        if (menus.Exists(m => m.id == canvasName))
                        {
                            continue;
                        }

                        Button[] buttons = canvas.GetComponentsInChildren<Button>(true);
                        if (buttons.Length > 0)
                        {
                            MenuData menu = new MenuData
                            {
                                id = canvas.gameObject.name,
                                name = canvas.gameObject.name,
                                hierarchy = GetHierarchyPath(canvas.transform),
                                buttons = CollectButtons(canvas.gameObject)
                            };
                            menus.Add(menu);
                            MelonLoader.MelonLogger.Msg($"[DebugExporter] Found canvas: {menu.id} with {buttons.Length} buttons");
                        }
                    }
                }

                MelonLoader.MelonLogger.Msg($"[DebugExporter] Total menus found: {menus.Count}");
            }
            catch (Exception ex)
            {
                MelonLoader.MelonLogger.Error($"[DebugExporter] Error collecting menus: {ex.Message}");
                MelonLoader.MelonLogger.Error($"[DebugExporter] Stack trace: {ex.StackTrace}");
            }

            return menus;
        }

        /// <summary>
        /// Collect all dialog data from the scene.
        /// </summary>
        private static List<DialogData> CollectDialogData()
        {
            List<DialogData> dialogs = new List<DialogData>();

            try
            {
                MelonLoader.MelonLogger.Msg("[DebugExporter] Starting dialog collection...");

                // Search for specific known dialog objects based on game code
                string[] dialogNames = new string[]
                {
                    "UIConf",           // Confirmation dialog
                    "UIDialog",         // Generic dialog
                    "UIMessage",        // Message dialog
                    "UIPopup",          // Popup dialog
                    "ConfirmDialog",    // Alternative confirmation
                    "MessageBox",       // Message box
                    "DialogBox"         // Dialog box
                };

                foreach (string dialogName in dialogNames)
                {
                    GameObject dialogObj = GameObject.Find(dialogName);
                    if (dialogObj != null)
                    {
                        DialogData dialog = new DialogData
                        {
                            id = dialogObj.name,
                            name = dialogObj.name,
                            hierarchy = GetHierarchyPath(dialogObj.transform),
                            messageField = FindMessageField(dialogObj),
                            buttons = CollectButtons(dialogObj)
                        };

                        dialogs.Add(dialog);
                        MelonLoader.MelonLogger.Msg($"[DebugExporter] Found dialog: {dialog.id} with {dialog.buttons.Count} buttons");
                    }
                }

                // Also search all GameObjects for dialog-like objects
                GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();
                MelonLoader.MelonLogger.Msg($"[DebugExporter] Searching {allObjects.Length} objects for dialogs...");

                foreach (GameObject obj in allObjects)
                {
                    string nameLower = obj.name.ToLower();

                    // Look for dialog-related objects that we haven't found yet
                    if ((nameLower.Contains("dialog") ||
                         nameLower.Contains("conf") ||
                         nameLower.Contains("popup") ||
                         nameLower.Contains("message")) &&
                        !dialogs.Exists(d => d.id == obj.name))
                    {
                        // Check if it has buttons (likely a dialog)
                        Button[] buttons = obj.GetComponentsInChildren<Button>(true);
                        if (buttons.Length > 0)
                        {
                            DialogData dialog = new DialogData
                            {
                                id = obj.name,
                                name = obj.name,
                                hierarchy = GetHierarchyPath(obj.transform),
                                messageField = FindMessageField(obj),
                                buttons = CollectButtons(obj)
                            };

                            dialogs.Add(dialog);
                            MelonLoader.MelonLogger.Msg($"[DebugExporter] Found dialog: {dialog.id} with {dialog.buttons.Count} buttons");
                        }
                    }
                }

                MelonLoader.MelonLogger.Msg($"[DebugExporter] Total dialogs found: {dialogs.Count}");
            }
            catch (Exception ex)
            {
                MelonLoader.MelonLogger.Error($"[DebugExporter] Error collecting dialogs: {ex.Message}");
                MelonLoader.MelonLogger.Error($"[DebugExporter] Stack trace: {ex.StackTrace}");
            }

            return dialogs;
        }

        /// <summary>
        /// Collect all buttons from a GameObject.
        /// </summary>
        private static List<ButtonData> CollectButtons(GameObject parent)
        {
            List<ButtonData> buttons = new List<ButtonData>();

            try
            {
                Button[] allButtons = parent.GetComponentsInChildren<Button>(true);
                MelonLoader.MelonLogger.Msg($"[DebugExporter] Collecting {allButtons.Length} buttons from {parent.name}");

                foreach (Button button in allButtons)
                {
                    string buttonText = GetButtonText(button.gameObject);

                    ButtonData buttonData = new ButtonData
                    {
                        id = button.gameObject.name,
                        text = buttonText,
                        image = GetButtonImageName(button.gameObject),
                        hierarchy = GetHierarchyPath(button.transform),
                        gameObject = button.gameObject // Store reference for image export
                    };

                    buttons.Add(buttonData);
                    MelonLoader.MelonLogger.Msg($"[DebugExporter]   Button: {buttonData.id} - Text: {buttonText}");
                }
            }
            catch (Exception ex)
            {
                MelonLoader.MelonLogger.Error($"[DebugExporter] Error collecting buttons: {ex.Message}");
            }

            return buttons;
        }

        /// <summary>
        /// Export collected data to JSON file.
        /// </summary>
        private static void ExportToJson(List<MenuData> menus, List<DialogData> dialogs, string langPath, string language)
        {
            try
            {
                string jsonPath = Path.Combine(langPath, "menu_export.json");

                StringBuilder sb = new StringBuilder();
                sb.AppendLine("{");
                sb.AppendLine("  \"export_info\": {");
                sb.AppendLine($"    \"timestamp\": \"{DateTime.Now:yyyy-MM-ddTHH:mm:ss}\",");
                sb.AppendLine($"    \"language\": \"{language}\"");
                sb.AppendLine("  },");
                sb.AppendLine("  \"menus\": [");

                // Export menus
                for (int i = 0; i < menus.Count; i++)
                {
                    MenuData menu = menus[i];
                    sb.AppendLine("    {");
                    sb.AppendLine($"      \"id\": \"{EscapeJson(menu.id)}\",");
                    sb.AppendLine($"      \"name\": \"{EscapeJson(menu.name)}\",");
                    sb.AppendLine($"      \"hierarchy\": \"{EscapeJson(menu.hierarchy)}\",");
                    sb.AppendLine("      \"buttons\": [");

                    for (int j = 0; j < menu.buttons.Count; j++)
                    {
                        ButtonData btn = menu.buttons[j];
                        sb.AppendLine("        {");
                        sb.AppendLine($"          \"id\": \"{EscapeJson(btn.id)}\",");
                        sb.AppendLine($"          \"text\": \"{EscapeJson(btn.text)}\",");
                        sb.AppendLine($"          \"image\": \"{EscapeJson(btn.image)}\",");
                        sb.AppendLine($"          \"hierarchy\": \"{EscapeJson(btn.hierarchy)}\"");
                        sb.Append("        }");
                        if (j < menu.buttons.Count - 1) sb.Append(",");
                        sb.AppendLine();
                    }

                    sb.AppendLine("      ]");
                    sb.Append("    }");
                    if (i < menus.Count - 1) sb.Append(",");
                    sb.AppendLine();
                }

                sb.AppendLine("  ],");
                sb.AppendLine("  \"dialogs\": [");

                // Export dialogs
                for (int i = 0; i < dialogs.Count; i++)
                {
                    DialogData dialog = dialogs[i];
                    sb.AppendLine("    {");
                    sb.AppendLine($"      \"id\": \"{EscapeJson(dialog.id)}\",");
                    sb.AppendLine($"      \"name\": \"{EscapeJson(dialog.name)}\",");
                    sb.AppendLine($"      \"hierarchy\": \"{EscapeJson(dialog.hierarchy)}\",");
                    sb.AppendLine($"      \"messageField\": \"{EscapeJson(dialog.messageField)}\",");
                    sb.AppendLine("      \"buttons\": [");

                    for (int j = 0; j < dialog.buttons.Count; j++)
                    {
                        ButtonData btn = dialog.buttons[j];
                        sb.AppendLine("        {");
                        sb.AppendLine($"          \"id\": \"{EscapeJson(btn.id)}\",");
                        sb.AppendLine($"          \"text\": \"{EscapeJson(btn.text)}\",");
                        sb.AppendLine($"          \"image\": \"{EscapeJson(btn.image)}\",");
                        sb.AppendLine($"          \"hierarchy\": \"{EscapeJson(btn.hierarchy)}\"");
                        sb.Append("        }");
                        if (j < dialog.buttons.Count - 1) sb.Append(",");
                        sb.AppendLine();
                    }

                    sb.AppendLine("      ]");
                    sb.Append("    }");
                    if (i < dialogs.Count - 1) sb.Append(",");
                    sb.AppendLine();
                }

                sb.AppendLine("  ]");
                sb.AppendLine("}");

                File.WriteAllText(jsonPath, sb.ToString());
                MelonLoader.MelonLogger.Msg($"[DebugExporter] Exported JSON: {jsonPath}");
            }
            catch (Exception ex)
            {
                MelonLoader.MelonLogger.Error($"[DebugExporter] Error exporting JSON: {ex.Message}");
            }
        }

        /// <summary>
        /// Export button images to debug folder.
        /// </summary>
        private static void ExportImages(List<MenuData> menus, List<DialogData> dialogs, string langPath)
        {
            try
            {
                string imagesPath = Path.Combine(langPath, "images");
                if (!Directory.Exists(imagesPath))
                {
                    Directory.CreateDirectory(imagesPath);
                }

                int totalButtons = 0;
                int successCount = 0;
                int failCount = 0;

                // Export menu button images
                foreach (MenuData menu in menus)
                {
                    MelonLoader.MelonLogger.Msg($"[DebugExporter] Exporting images for menu: {menu.id}");
                    foreach (ButtonData button in menu.buttons)
                    {
                        totalButtons++;
                        bool success = ExportButtonImage(button, imagesPath);
                        if (success)
                            successCount++;
                        else
                            failCount++;
                    }
                }

                // Export dialog button images
                foreach (DialogData dialog in dialogs)
                {
                    MelonLoader.MelonLogger.Msg($"[DebugExporter] Exporting images for dialog: {dialog.id}");
                    foreach (ButtonData button in dialog.buttons)
                    {
                        totalButtons++;
                        bool success = ExportButtonImage(button, imagesPath);
                        if (success)
                            successCount++;
                        else
                            failCount++;
                    }
                }

                MelonLoader.MelonLogger.Msg($"[DebugExporter] Image export summary:");
                MelonLoader.MelonLogger.Msg($"[DebugExporter]   Total buttons: {totalButtons}");
                MelonLoader.MelonLogger.Msg($"[DebugExporter]   Successfully exported: {successCount}");
                MelonLoader.MelonLogger.Msg($"[DebugExporter]   Failed to export: {failCount}");
                MelonLoader.MelonLogger.Msg($"[DebugExporter]   Images saved to: {imagesPath}");
            }
            catch (Exception ex)
            {
                MelonLoader.MelonLogger.Error($"[DebugExporter] Error exporting images: {ex.Message}");
            }
        }

        /// <summary>
        /// Export a single button's image.
        /// Returns true if successful, false otherwise.
        /// </summary>
        private static bool ExportButtonImage(ButtonData button, string imagesPath)
        {
            try
            {
                if (button.gameObject == null)
                {
                    MelonLoader.MelonLogger.Msg($"[DebugExporter]   {button.id}: No GameObject reference");
                    return false;
                }

                // Get Image component
                Image image = button.gameObject.GetComponent<Image>();
                if (image == null)
                {
                    MelonLoader.MelonLogger.Msg($"[DebugExporter]   {button.id}: No Image component");
                    return false;
                }

                if (image.sprite == null)
                {
                    MelonLoader.MelonLogger.Msg($"[DebugExporter]   {button.id}: Image has no sprite");
                    return false;
                }

                // Get sprite texture
                Texture2D texture = image.sprite.texture;
                if (texture == null)
                {
                    MelonLoader.MelonLogger.Msg($"[DebugExporter]   {button.id}: Sprite has no texture");
                    return false;
                }

                // Try to encode directly first
                try
                {
                    byte[] pngData = ImageConversion.EncodeToPNG(texture);
                    if (pngData != null && pngData.Length > 0)
                    {
                        string imagePath = Path.Combine(imagesPath, button.image);
                        File.WriteAllBytes(imagePath, pngData);
                        MelonLoader.MelonLogger.Msg($"[DebugExporter]   {button.id}: ✓ Exported {button.image} ({pngData.Length} bytes)");
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    // Texture not readable, try RenderTexture method
                    MelonLoader.MelonLogger.Msg($"[DebugExporter]   {button.id}: Direct encode failed ({ex.Message}), trying RenderTexture...");
                }

                // Create a temporary RenderTexture to read the texture
                RenderTexture tmp = RenderTexture.GetTemporary(
                    texture.width,
                    texture.height,
                    0,
                    RenderTextureFormat.Default,
                    RenderTextureReadWrite.Linear);

                Graphics.Blit(texture, tmp);
                RenderTexture previous = RenderTexture.active;
                RenderTexture.active = tmp;

                Texture2D readableTexture = new Texture2D(texture.width, texture.height);
                readableTexture.ReadPixels(new Rect(0, 0, tmp.width, tmp.height), 0, 0);
                readableTexture.Apply();

                RenderTexture.active = previous;
                RenderTexture.ReleaseTemporary(tmp);

                byte[] pngData2 = ImageConversion.EncodeToPNG(readableTexture);
                if (pngData2 != null && pngData2.Length > 0)
                {
                    string imagePath = Path.Combine(imagesPath, button.image);
                    File.WriteAllBytes(imagePath, pngData2);
                    MelonLoader.MelonLogger.Msg($"[DebugExporter]   {button.id}: ✓ Exported via RenderTexture {button.image} ({pngData2.Length} bytes)");
                    return true;
                }
                else
                {
                    MelonLoader.MelonLogger.Warning($"[DebugExporter]   {button.id}: ✗ Failed to encode PNG");
                    return false;
                }
            }
            catch (Exception ex)
            {
                MelonLoader.MelonLogger.Error($"[DebugExporter]   {button.id}: ✗ Error - {ex.Message}");
                return false;
            }
        }

        // Helper methods
        private static string[] GetSupportedLanguages()
        {
            return new string[] { "en", "cn", "tc", "jp" };
        }

        private static string GetCurrentLanguage()
        {
            try
            {
                // Convert EngineMain.Language enum to string
                EngineMain.Language lang = EngineMain.lang;
                switch (lang)
                {
                    case EngineMain.Language.en: return "en";
                    case EngineMain.Language.cn: return "cn";
                    case EngineMain.Language.tc: return "tc";
                    case EngineMain.Language.jp: return "jp";
                    default: return "en";
                }
            }
            catch { }
            return "en";
        }

        private static void SetGameLanguage(string language)
        {
            try
            {
                // Convert string to EngineMain.Language enum
                EngineMain.Language lang = EngineMain.Language.en;
                switch (language)
                {
                    case "en": lang = EngineMain.Language.en; break;
                    case "cn": lang = EngineMain.Language.cn; break;
                    case "tc": lang = EngineMain.Language.tc; break;
                    case "jp": lang = EngineMain.Language.jp; break;
                }
                EngineMain.lang = lang;
            }
            catch (Exception ex)
            {
                MelonLoader.MelonLogger.Error($"[DebugExporter] Error setting language: {ex.Message}");
            }
        }

        private static bool IsMenuCanvas(GameObject obj)
        {
            // Check if this is a menu canvas (UITitle, UIConf, etc.)
            string name = obj.name.ToLower();
            return name.Contains("ui") || name.Contains("menu") || name.Contains("canvas");
        }

        private static bool IsDialogObject(GameObject obj)
        {
            // Check if this is a dialog object
            string name = obj.name.ToLower();
            return name.Contains("dialog") || name.Contains("confirm") || name.Contains("popup");
        }

        private static string GetMenuName(GameObject obj)
        {
            return obj.name;
        }

        private static string GetDialogName(GameObject obj)
        {
            return obj.name;
        }

        private static string FindMessageField(GameObject obj)
        {
            // Try to find text field for dialog message
            TextMeshProUGUI[] tmpTexts = obj.GetComponentsInChildren<TextMeshProUGUI>(true);
            foreach (var tmp in tmpTexts)
            {
                if (tmp.gameObject.name.ToLower().Contains("message") ||
                    tmp.gameObject.name.ToLower().Contains("text"))
                {
                    return tmp.gameObject.name;
                }
            }
            return "";
        }

        private static string GetButtonText(GameObject button)
        {
            // Try TextMeshProUGUI first
            TextMeshProUGUI tmp = button.GetComponentInChildren<TextMeshProUGUI>();
            if (tmp != null && !string.IsNullOrEmpty(tmp.text))
            {
                return tmp.text;
            }

            // Try UI.Text
            Text text = button.GetComponentInChildren<Text>();
            if (text != null && !string.IsNullOrEmpty(text.text))
            {
                return text.text;
            }

            return button.name;
        }

        private static string GetButtonImageName(GameObject button)
        {
            Image image = button.GetComponent<Image>();
            if (image != null && image.sprite != null)
            {
                return $"{button.name}_{image.sprite.name}.png";
            }
            return $"{button.name}.png";
        }

        private static string GetHierarchyPath(Transform transform)
        {
            string path = transform.name;
            Transform parent = transform.parent;

            while (parent != null)
            {
                path = parent.name + "/" + path;
                parent = parent.parent;
            }

            return path;
        }

        private static string EscapeJson(string str)
        {
            if (string.IsNullOrEmpty(str)) return "";
            return str.Replace("\\", "\\\\").Replace("\"", "\\\"").Replace("\n", "\\n").Replace("\r", "\\r");
        }

        // Data structures
        private class MenuData
        {
            public string id;
            public string name;
            public string hierarchy;
            public List<ButtonData> buttons = new List<ButtonData>();
        }

        private class DialogData
        {
            public string id;
            public string name;
            public string hierarchy;
            public string messageField;
            public List<ButtonData> buttons = new List<ButtonData>();
        }

        private class ButtonData
        {
            public string id;
            public string text;
            public string image;
            public string hierarchy;
            public GameObject gameObject; // Store reference for image export
        }
    }
}
