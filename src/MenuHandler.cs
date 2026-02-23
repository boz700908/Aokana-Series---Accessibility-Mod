using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace AokanaAccess
{
    /// <summary>
    /// Handles menu navigation announcements.
    /// </summary>
    public static class MenuHandler
    {
        private static bool _menuActive = false;
        private static GameObject _lastFocusedObject = null;
        private static string _lastAnnouncement = string.Empty;
        private static bool _confirmDialogJustOpened = false;
        private static bool _suppressNextAnnouncement = false;
        private static float _lastDialogOpenTime = 0f;

        // Hardcoded button translations (text extracted from game button images)
        // Format: [buttonName][language] = "text from image"
        private static Dictionary<string, Dictionary<string, string>> _buttonTranslations = new Dictionary<string, Dictionary<string, string>>
        {
            // Title menu buttons
            { "btnNew", new Dictionary<string, string> {
                { "cn", "开始游戏" },
                { "en", "New Game" },
                { "jp", "はじめから" },
                { "tc", "開始遊戲" }
            }},
            { "btnLoad", new Dictionary<string, string> {
                { "cn", "读取进度" },
                { "en", "Load" },
                { "jp", "ロード" },
                { "tc", "讀取" }
            }},
            { "btnConfig", new Dictionary<string, string> {
                { "cn", "系统设置" },
                { "en", "Config" },
                { "jp", "設定" },
                { "tc", "設定" }
            }},
            { "btnQuit", new Dictionary<string, string> {
                { "cn", "退出游戏" },
                { "en", "Quit" },
                { "jp", "終了" },
                { "tc", "退出" }
            }},
            { "btnExtra", new Dictionary<string, string> {
                { "cn", "鉴赏" },
                { "en", "Extra" },
                { "jp", "エクストラ" },
                { "tc", "附加內容" }
            }},
            { "btnGallery", new Dictionary<string, string> {
                { "cn", "CG鉴赏" },
                { "en", "Gallery" },
                { "jp", "ギャラリー" },
                { "tc", "鑑賞" }
            }},
            { "btnVbmarks", new Dictionary<string, string> {
                { "cn", "语音书签" },
                { "en", "Voice Bookmarks" },
                { "jp", "ボイスブックマーク" },
                { "tc", "語音書籤" }
            }},
            { "btnClose", new Dictionary<string, string> {
                { "cn", "关闭" },
                { "en", "Close" },
                { "jp", "閉じる" },
                { "tc", "關閉" }
            }},
            // In-game buttons
            { "btn_auto", new Dictionary<string, string> {
                { "cn", "自动" },
                { "en", "Auto" },
                { "jp", "オート" },
                { "tc", "自動" }
            }},
            { "btn_skip", new Dictionary<string, string> {
                { "cn", "跳过" },
                { "en", "Skip" },
                { "jp", "スキップ" },
                { "tc", "跳過" }
            }},
            { "btn_backlog", new Dictionary<string, string> {
                { "cn", "历史记录" },
                { "en", "Backlog" },
                { "jp", "バックログ" },
                { "tc", "回顧" }
            }},
            { "btn_voice", new Dictionary<string, string> {
                { "cn", "语音" },
                { "en", "Voice" },
                { "jp", "ボイス" },
                { "tc", "語音" }
            }},
            { "btn_config", new Dictionary<string, string> {
                { "cn", "系统设置" },
                { "en", "Config" },
                { "jp", "設定" },
                { "tc", "設定" }
            }},
            { "btn_hide", new Dictionary<string, string> {
                { "cn", "隐藏" },
                { "en", "Hide" },
                { "jp", "非表示" },
                { "tc", "隱藏" }
            }},
            { "btn_vbmark", new Dictionary<string, string> {
                { "cn", "语音书签" },
                { "en", "Voice Bookmark" },
                { "jp", "ボイスブックマーク" },
                { "tc", "語音書籤" }
            }},
            { "btn_vbmnew", new Dictionary<string, string> {
                { "cn", "语音书签" },
                { "en", "New Voice Bookmark" },
                { "jp", "新規ボイスブックマーク" },
                { "tc", "新建語音書籤" }
            }},
            { "btn_save", new Dictionary<string, string> {
                { "cn", "保存" },
                { "en", "Save" },
                { "jp", "セーブ" },
                { "tc", "儲存" }
            }},
            { "btn_load", new Dictionary<string, string> {
                { "cn", "读取" },
                { "en", "Load" },
                { "jp", "ロード" },
                { "tc", "讀取" }
            }},
            { "btn_qsave", new Dictionary<string, string> {
                { "cn", "快速保存" },
                { "en", "Quick Save" },
                { "jp", "クイックセーブ" },
                { "tc", "快速儲存" }
            }},
            { "btn_qload", new Dictionary<string, string> {
                { "cn", "快速读取" },
                { "en", "Quick Load" },
                { "jp", "クイックロード" },
                { "tc", "快速讀取" }
            }},
            { "btn_quit", new Dictionary<string, string> {
                { "cn", "退出" },
                { "en", "Quit" },
                { "jp", "終了" },
                { "tc", "退出" }
            }},
            // Page buttons (save/load pages)
            { "page0", new Dictionary<string, string> {
                { "cn", "1" },
                { "en", "1" },
                { "jp", "1" },
                { "tc", "1" }
            }},
            { "page1", new Dictionary<string, string> {
                { "cn", "2" },
                { "en", "2" },
                { "jp", "2" },
                { "tc", "2" }
            }},
            { "page2", new Dictionary<string, string> {
                { "cn", "3" },
                { "en", "3" },
                { "jp", "3" },
                { "tc", "3" }
            }},
            { "page3", new Dictionary<string, string> {
                { "cn", "4" },
                { "en", "4" },
                { "jp", "4" },
                { "tc", "4" }
            }},
            { "page4", new Dictionary<string, string> {
                { "cn", "5" },
                { "en", "5" },
                { "jp", "5" },
                { "tc", "5" }
            }},
            { "page5", new Dictionary<string, string> {
                { "cn", "6" },
                { "en", "6" },
                { "jp", "6" },
                { "tc", "6" }
            }},
            { "page6", new Dictionary<string, string> {
                { "cn", "7" },
                { "en", "7" },
                { "jp", "7" },
                { "tc", "7" }
            }},
            { "page7", new Dictionary<string, string> {
                { "cn", "8" },
                { "en", "8" },
                { "jp", "8" },
                { "tc", "8" }
            }},
            { "page8", new Dictionary<string, string> {
                { "cn", "9" },
                { "en", "9" },
                { "jp", "9" },
                { "tc", "9" }
            }},
            { "page9", new Dictionary<string, string> {
                { "cn", "10" },
                { "en", "10" },
                { "jp", "10" },
                { "tc", "10" }
            }},
            // Navigation buttons
            { "navNext", new Dictionary<string, string> {
                { "cn", "下一页" },
                { "en", "Next" },
                { "jp", "次へ" },
                { "tc", "下一頁" }
            }},
            { "navPrev", new Dictionary<string, string> {
                { "cn", "上一页" },
                { "en", "Previous" },
                { "jp", "前へ" },
                { "tc", "上一頁" }
            }},
            // Delete buttons (all same text)
            { "del0", new Dictionary<string, string> {
                { "cn", "删除" },
                { "en", "Delete" },
                { "jp", "削除" },
                { "tc", "刪除" }
            }},
            { "del1", new Dictionary<string, string> {
                { "cn", "删除" },
                { "en", "Delete" },
                { "jp", "削除" },
                { "tc", "刪除" }
            }},
            { "del2", new Dictionary<string, string> {
                { "cn", "删除" },
                { "en", "Delete" },
                { "jp", "削除" },
                { "tc", "刪除" }
            }},
            { "del3", new Dictionary<string, string> {
                { "cn", "删除" },
                { "en", "Delete" },
                { "jp", "削除" },
                { "tc", "刪除" }
            }},
            { "del4", new Dictionary<string, string> {
                { "cn", "删除" },
                { "en", "Delete" },
                { "jp", "削除" },
                { "tc", "刪除" }
            }},
            { "del5", new Dictionary<string, string> {
                { "cn", "删除" },
                { "en", "Delete" },
                { "jp", "削除" },
                { "tc", "刪除" }
            }},
            { "del6", new Dictionary<string, string> {
                { "cn", "删除" },
                { "en", "Delete" },
                { "jp", "削除" },
                { "tc", "刪除" }
            }},
            { "del7", new Dictionary<string, string> {
                { "cn", "删除" },
                { "en", "Delete" },
                { "jp", "削除" },
                { "tc", "刪除" }
            }},
            { "del8", new Dictionary<string, string> {
                { "cn", "删除" },
                { "en", "Delete" },
                { "jp", "削除" },
                { "tc", "刪除" }
            }},
            { "del9", new Dictionary<string, string> {
                { "cn", "删除" },
                { "en", "Delete" },
                { "jp", "削除" },
                { "tc", "刪除" }
            }},
            // Settings tabs
            { "tabText", new Dictionary<string, string> {
                { "cn", "文字" },
                { "en", "Text" },
                { "jp", "テキスト" },
                { "tc", "文字" }
            }},
            { "tabSound", new Dictionary<string, string> {
                { "cn", "音效" },
                { "en", "Sound" },
                { "jp", "サウンド" },
                { "tc", "聲音" }
            }},
            { "tabVoice", new Dictionary<string, string> {
                { "cn", "语音" },
                { "en", "Voice" },
                { "jp", "ボイス" },
                { "tc", "語音" }
            }},
            { "tabVisual", new Dictionary<string, string> {
                { "cn", "画面" },
                { "en", "Visual" },
                { "jp", "ビジュアル" },
                { "tc", "畫面" }
            }},
            { "title", new Dictionary<string, string> {
                { "cn", "标题" },
                { "en", "Title" },
                { "jp", "タイトル" },
                { "tc", "標題" }
            }},
            { "close", new Dictionary<string, string> {
                { "cn", "关闭" },
                { "en", "Close" },
                { "jp", "閉じる" },
                { "tc", "關閉" }
            }},
            // Confirmation buttons
            { "0confirm", new Dictionary<string, string> {
                { "cn", "否" },
                { "en", "No" },
                { "jp", "いいえ" },
                { "tc", "否" }
            }},
            { "1confirm", new Dictionary<string, string> {
                { "cn", "是" },
                { "en", "Yes" },
                { "jp", "はい" },
                { "tc", "是" }
            }},
            // Choice buttons
            { "choice0", new Dictionary<string, string> {
                { "cn", "明日香" },
                { "en", "Asuka" },
                { "jp", "明日香" },
                { "tc", "明日香" }
            }},
            { "choice1", new Dictionary<string, string> {
                { "cn", "美咲" },
                { "en", "Misaki" },
                { "jp", "美咲" },
                { "tc", "美咲" }
            }},
            { "choice2", new Dictionary<string, string> {
                { "cn", "莉佳" },
                { "en", "Rika" },
                { "jp", "莉佳" },
                { "tc", "莉佳" }
            }},
            // Extra menu
            { "exui", new Dictionary<string, string> {
                { "cn", "关闭" },
                { "en", "Close" },
                { "jp", "閉じる" },
                { "tc", "關閉" }
            }},
            // Settings controls (keep old keys for compatibility)
            { "borderless", new Dictionary<string, string> {
                { "cn", "无边框" },
                { "en", "Borderless" },
                { "jp", "ボーダーレス" },
                { "tc", "無邊框" }
            }},
            { "off", new Dictionary<string, string> {
                { "cn", "窗口" },
                { "en", "Windowed" },
                { "jp", "ウィンドウ" },
                { "tc", "視窗" }
            }},
            { "on", new Dictionary<string, string> {
                { "cn", "全屏" },
                { "en", "Fullscreen" },
                { "jp", "フルスクリーン" },
                { "tc", "全螢幕" }
            }},
            { "cn", new Dictionary<string, string> {
                { "cn", "简体中文" },
                { "en", "Simplified Chinese" },
                { "jp", "簡体字中国語" },
                { "tc", "簡體中文" }
            }},
            { "tc", new Dictionary<string, string> {
                { "cn", "繁体中文" },
                { "en", "Traditional Chinese" },
                { "jp", "繁体字中国語" },
                { "tc", "繁體中文" }
            }},
            { "jp", new Dictionary<string, string> {
                { "cn", "日语" },
                { "en", "Japanese" },
                { "jp", "日本語" },
                { "tc", "日語" }
            }},
            { "en", new Dictionary<string, string> {
                { "cn", "英语" },
                { "en", "English" },
                { "jp", "英語" },
                { "tc", "英語" }
            }}
        };

        // Confirmation dialog image path to text mapping (dialogs use sprite images for text)
        private static Dictionary<string, string> _dialogTexts = new Dictionary<string, string>
        {
            { "SGDialog990000", "dialog_quit" },
            { "SGDialog990005", "dialog_quick_load" },
            { "SGDialog990010", "dialog_return_title" },
            { "SGDialog990015", "dialog_overwrite" }
        };

        /// <summary>
        /// Called when a menu is opened.
        /// </summary>
        public static void OnMenuOpened(string menuName)
        {
            _menuActive = true;
            _lastFocusedObject = null;
            _lastAnnouncement = string.Empty;

            DebugLogger.Log($"Menu opened: {menuName}");
            ScreenReader.Speak(menuName, interrupt: true);
        }

        /// <summary>
        /// Called when a confirmation dialog is opened.
        /// </summary>
        public static void OnConfirmDialogOpened(string bgpath)
        {
            _menuActive = true;
            _confirmDialogJustOpened = true;
            _lastDialogOpenTime = Time.time;

            // Get current focus and store it without announcing
            if (EventSystem.current != null)
            {
                _lastFocusedObject = EventSystem.current.currentSelectedGameObject;
                DebugLogger.Log($"Dialog opened, initial focus: {(_lastFocusedObject != null ? _lastFocusedObject.name : "null")}");
            }

            _lastAnnouncement = string.Empty;

            DebugLogger.Log($"Confirmation dialog opened with bgpath: {bgpath}");

            // Only announce dialog text, not buttons
            if (_dialogTexts.TryGetValue(bgpath, out string dialogKey))
            {
                string dialogText = Loc.Get(dialogKey);
                DebugLogger.Log($"Found dialog text: {dialogText}");
                ScreenReader.Speak(dialogText, interrupt: true);
            }
            else
            {
                DebugLogger.Log($"No dialog text mapping found for: {bgpath}");
                ScreenReader.Speak(Loc.Get("dialog_generic"), interrupt: true);
            }
        }

        /// <summary>
        /// Called when a menu is closed.
        /// </summary>
        public static void OnMenuClosed(string menuName = "")
        {
            DebugLogger.Log($"OnMenuClosed called for: {menuName}");

            // Announce menu close if provided
            if (!string.IsNullOrEmpty(menuName))
            {
                AnnounceMenuCloseInternal(menuName);
            }

            _menuActive = false;
            _lastFocusedObject = null;
            _lastAnnouncement = string.Empty;
            _confirmDialogJustOpened = false;

            DebugLogger.Log("Menu closed");
        }

        /// <summary>
        /// Called when a confirmation dialog is closed (but parent menu may still be active).
        /// </summary>
        public static void OnConfirmDialogClosed()
        {
            DebugLogger.Log("Confirmation dialog closing");

            // Clear focus tracking to prevent announcing button on close
            _lastFocusedObject = null;
            _lastAnnouncement = string.Empty;
            _confirmDialogJustOpened = false;

            DebugLogger.Log("Confirmation dialog closed, menu still active");
        }

        /// <summary>
        /// Announce menu close without resetting MenuHandler state.
        /// Used for submenus where parent menu remains active.
        /// </summary>
        public static void AnnounceMenuClose(string menuName)
        {
            DebugLogger.Log($"AnnounceMenuClose called for: {menuName}");

            AnnounceMenuCloseInternal(menuName);

            // Clear focus tracking but keep menu active for parent menu
            _lastFocusedObject = null;
            _lastAnnouncement = string.Empty;
        }

        /// <summary>
        /// Internal method to announce menu close with dialog suppression logic.
        /// </summary>
        private static void AnnounceMenuCloseInternal(string menuName)
        {
            // Check if a dialog just opened (within 0.1 seconds)
            bool dialogJustOpened = Time.time - _lastDialogOpenTime < 0.1f;

            if (dialogJustOpened)
            {
                DebugLogger.Log("Menu close announcement suppressed - dialog just opened");
            }
            else if (!string.IsNullOrEmpty(menuName))
            {
                // Announce menu close
                string closeAnnouncement = Loc.Get("menu_closed", menuName);
                ScreenReader.Speak(closeAnnouncement, interrupt: true);
                DebugLogger.Log($"Announced menu close: {closeAnnouncement}");
            }
        }

        /// <summary>
        /// Suppress the next focus change announcement.
        /// Used when programmatically setting focus (e.g., auto-focus on tab switch).
        /// </summary>
        public static void SuppressNextAnnouncement()
        {
            _suppressNextAnnouncement = true;
            DebugLogger.Log("Next announcement will be suppressed");
        }

        /// <summary>
        /// Check for focus changes and announce new focused element.
        /// Call this from Update patches.
        /// </summary>
        public static void CheckFocusChange()
        {
            if (EventSystem.current == null)
            {
                return;
            }

            GameObject currentFocus = EventSystem.current.currentSelectedGameObject;

            // Log focus state for debugging
            if (currentFocus != null && currentFocus != _lastFocusedObject)
            {
                DebugLogger.Log($"CheckFocusChange: _menuActive={_menuActive}, currentFocus={currentFocus.name}, lastFocus={(_lastFocusedObject != null ? _lastFocusedObject.name : "null")}");
            }

            if (!_menuActive)
            {
                return;
            }

            // For confirmation dialogs that just opened, skip announcements until focus is set
            if (_confirmDialogJustOpened)
            {
                // Wait until focus is actually set to a button
                if (currentFocus != null)
                {
                    _lastFocusedObject = currentFocus;
                    _confirmDialogJustOpened = false;
                    DebugLogger.Log($"Dialog just opened, initial focus set to: {currentFocus.name}, will announce on next change");
                }
                return;
            }

            // Only announce when focus actually changes (keyboard navigation)
            if (currentFocus != _lastFocusedObject && currentFocus != null)
            {
                DebugLogger.Log($"Focus changed from {(_lastFocusedObject != null ? _lastFocusedObject.name : "null")} to {currentFocus.name}");
                _lastFocusedObject = currentFocus;
                OnFocusChanged(currentFocus);
            }
        }

        /// <summary>
        /// Called when focus changes to a new UI element.
        /// </summary>
        private static void OnFocusChanged(GameObject newFocus)
        {
            if (newFocus == null)
            {
                return;
            }

            // Check if we should suppress this announcement (e.g., auto-focus)
            if (_suppressNextAnnouncement)
            {
                _suppressNextAnnouncement = false;
                DebugLogger.Log("Announcement suppressed for auto-focus");
                return;
            }

            string announcement = GetElementAnnouncement(newFocus);

            if (!string.IsNullOrEmpty(announcement) && announcement != _lastAnnouncement)
            {
                _lastAnnouncement = announcement;
                ScreenReader.Speak(announcement, interrupt: true);
                DebugLogger.Log($"Announced menu item: {announcement}");
            }
        }

        /// <summary>
        /// Handle input for menu features.
        /// </summary>
        public static void HandleInput()
        {
            // Check for focus changes (works for all menus)
            CheckFocusChange();

            if (!_menuActive)
            {
                return;
            }

            // F7 - Repeat current menu item
            if (Input.GetKeyDown(KeyCode.F7))
            {
                RepeatCurrentItem();
            }
        }

        /// <summary>
        /// Repeat the currently focused menu item.
        /// </summary>
        private static void RepeatCurrentItem()
        {
            // Check if settings menu is active and has an announcement
            if (SettingsHandler.IsActive())
            {
                SettingsHandler.RepeatLastAnnouncement();
                return;
            }

            // Use MenuHandler's last announcement
            if (!string.IsNullOrEmpty(_lastAnnouncement))
            {
                ScreenReader.Speak(_lastAnnouncement, interrupt: true);
            }
            // If no announcement, do nothing (silent)
        }

        /// <summary>
        /// Get announcement text for a UI element.
        /// </summary>
        private static string GetElementAnnouncement(GameObject obj)
        {
            if (obj == null)
            {
                return string.Empty;
            }

            DebugLogger.Log($"Getting announcement for object: {obj.name}");

            // Check if it's a voice bookmark item button (item0-item9)
            if (obj.name.StartsWith("item") && obj.name.Length <= 5)
            {
                string itemText = GetVoiceBookmarkItemText(obj);
                if (!string.IsNullOrEmpty(itemText))
                {
                    DebugLogger.Log($"Found voice bookmark item text: {itemText}");
                    return itemText;
                }
            }

            // Check if it's a page button (page0-page14 for voice bookmarks, page0-page9 for save/load)
            if (obj.name.StartsWith("page") && obj.name.Length <= 6)
            {
                // First try hardcoded translation
                if (_buttonTranslations.ContainsKey(obj.name))
                {
                    string currentLang = Loc.CurrentLanguage;
                    string pageText = GetHardcodedButtonText(obj.name, currentLang);
                    DebugLogger.Log($"Found hardcoded page button: {obj.name} = {pageText}");
                    return pageText;
                }

                // Fallback to old format
                string pageNumStr = obj.name.Substring(4);
                if (int.TryParse(pageNumStr, out int pageNum))
                {
                    string pageText = Loc.Get("page_format", pageNum + 1);
                    DebugLogger.Log($"Found page button: {pageText}");
                    return pageText;
                }
            }

            // Check if it's a button with hardcoded translation
            if (_buttonTranslations.ContainsKey(obj.name))
            {
                string currentLang = Loc.CurrentLanguage;
                string buttonLabel = GetHardcodedButtonText(obj.name, currentLang);

                DebugLogger.Log($"Found hardcoded button text: {obj.name} = {buttonLabel} (lang: {currentLang})");

                // Check if this is a slider control - announce with current value
                var slider = obj.GetComponent<UnityEngine.UI.Slider>();
                if (slider != null)
                {
                    int percentage = (int)slider.value;
                    string sliderAnnouncement = $"{buttonLabel}: {percentage}%";
                    DebugLogger.Log($"Found slider with value: {sliderAnnouncement}");
                    return sliderAnnouncement;
                }

                return buttonLabel;
            }

            // Try to get text from the object itself first
            TextMeshProUGUI tmpText = obj.GetComponent<TextMeshProUGUI>();
            if (tmpText != null && !string.IsNullOrEmpty(tmpText.text))
            {
                DebugLogger.Log($"Found TextMeshProUGUI on self: {tmpText.text}");
                return CleanText(tmpText.text);
            }

            // Try to get text from children (search deeply, including inactive)
            tmpText = obj.GetComponentInChildren<TextMeshProUGUI>(true);
            if (tmpText != null && !string.IsNullOrEmpty(tmpText.text))
            {
                DebugLogger.Log($"Found TextMeshProUGUI in children: {tmpText.text}");
                return CleanText(tmpText.text);
            }

            // Try parent object
            if (obj.transform.parent != null)
            {
                tmpText = obj.transform.parent.GetComponentInChildren<TextMeshProUGUI>(true);
                if (tmpText != null && !string.IsNullOrEmpty(tmpText.text))
                {
                    DebugLogger.Log($"Found TextMeshProUGUI in parent: {tmpText.text}");
                    return CleanText(tmpText.text);
                }
            }

            // Try legacy Text component
            Text legacyText = obj.GetComponentInChildren<Text>(true);
            if (legacyText != null && !string.IsNullOrEmpty(legacyText.text))
            {
                DebugLogger.Log($"Found Text in children: {legacyText.text}");
                return CleanText(legacyText.text);
            }

            // Try to get control value for sliders and toggles
            string controlValue = GetControlValue(obj);
            if (!string.IsNullOrEmpty(controlValue))
            {
                return controlValue;
            }

            // Fallback to object name
            DebugLogger.Log($"No text found, using object name: {obj.name}");
            return obj.name;
        }

        /// <summary>
        /// Get hardcoded button text based on button name and current language.
        /// </summary>
        private static string GetHardcodedButtonText(string buttonName, string language)
        {
            if (_buttonTranslations.TryGetValue(buttonName, out var translations))
            {
                if (translations.TryGetValue(language, out string text))
                {
                    return text;
                }
                // Fallback to English if current language not found
                if (translations.TryGetValue("en", out string fallback))
                {
                    return fallback;
                }
            }
            // Ultimate fallback to button name
            return buttonName;
        }

        /// <summary>
        /// Get text for voice bookmark item button.
        /// </summary>
        private static string GetVoiceBookmarkItemText(GameObject itemButton)
        {
            try
            {
                // Extract item number from button name (item0 -> 0)
                string itemNumStr = itemButton.name.Substring(4);
                if (!int.TryParse(itemNumStr, out int itemNum))
                {
                    DebugLogger.Log($"Failed to parse item number from: {itemButton.name}");
                    return string.Empty;
                }

                // Hierarchy: VBookmarkCanvas > VbGrp > bookmarks > item0
                // We need to go up 3 levels to reach VBookmarkCanvas
                Transform canvas = itemButton.transform.parent?.parent?.parent;
                if (canvas == null)
                {
                    DebugLogger.Log($"Failed to find canvas (parent.parent.parent is null)");
                    return string.Empty;
                }

                DebugLogger.Log($"Canvas found: {canvas.name}");

                // Look for tx{itemNum} object in canvas
                Transform txTransform = canvas.Find($"tx{itemNum}");
                if (txTransform == null)
                {
                    DebugLogger.Log($"Failed to find tx{itemNum} in canvas {canvas.name}");
                    return string.Empty;
                }

                DebugLogger.Log($"Found tx{itemNum}");

                // Get the TextMeshProUGUI component
                TextMeshProUGUI tmpText = txTransform.GetComponent<TextMeshProUGUI>();
                if (tmpText == null || string.IsNullOrEmpty(tmpText.text))
                {
                    // Empty slot
                    DebugLogger.Log($"Slot {itemNum + 1} is empty");
                    return Loc.Get("slot_empty", itemNum + 1);
                }

                // Get the dispnum (slot number display)
                TextMeshProUGUI dispnum = txTransform.GetChild(0)?.GetComponent<TextMeshProUGUI>();
                string slotNum = dispnum != null ? dispnum.text : $"{itemNum + 1}";

                string result = Loc.Get("slot_format", slotNum, CleanText(tmpText.text));
                DebugLogger.Log($"Voice bookmark text: {result}");
                return result;
            }
            catch (System.Exception ex)
            {
                DebugLogger.Log($"Error getting voice bookmark text: {ex.Message}");
                return string.Empty;
            }
        }

        /// <summary>
        /// Clean text by removing formatting tags and trimming.
        /// </summary>
        private static string CleanText(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return string.Empty;
            }

            // Remove TextMeshPro tags like <color>, <size>, etc.
            string cleaned = System.Text.RegularExpressions.Regex.Replace(text, "<[^>]+>", "");
            return cleaned.Trim();
        }

        /// <summary>
        /// Get value announcement for controls (sliders, toggles).
        /// </summary>
        private static string GetControlValue(GameObject obj)
        {
            // Check for Slider
            Slider slider = obj.GetComponent<Slider>();
            if (slider != null)
            {
                // Try to find label
                string label = GetElementText(obj);
                return $"{label}: {slider.value:F1}";
            }

            // Check for Toggle
            Toggle toggle = obj.GetComponent<Toggle>();
            if (toggle != null)
            {
                string label = GetElementText(obj);
                string stateKey = toggle.isOn ? "toggle_on" : "toggle_off";
                string state = Loc.Get(stateKey);
                return $"{label}: {state}";
            }

            return string.Empty;
        }

        /// <summary>
        /// Get text from UI element (helper method).
        /// </summary>
        private static string GetElementText(GameObject obj)
        {
            if (obj == null)
            {
                return string.Empty;
            }

            // Try self
            TextMeshProUGUI tmpText = obj.GetComponent<TextMeshProUGUI>();
            if (tmpText != null && !string.IsNullOrEmpty(tmpText.text))
            {
                return CleanText(tmpText.text);
            }

            Text legacyText = obj.GetComponent<Text>();
            if (legacyText != null && !string.IsNullOrEmpty(legacyText.text))
            {
                return CleanText(legacyText.text);
            }

            // Try children
            tmpText = obj.GetComponentInChildren<TextMeshProUGUI>(true);
            if (tmpText != null && !string.IsNullOrEmpty(tmpText.text))
            {
                return CleanText(tmpText.text);
            }

            legacyText = obj.GetComponentInChildren<Text>(true);
            if (legacyText != null && !string.IsNullOrEmpty(legacyText.text))
            {
                return CleanText(legacyText.text);
            }

            // Try parent
            if (obj.transform.parent != null)
            {
                tmpText = obj.transform.parent.GetComponentInChildren<TextMeshProUGUI>(true);
                if (tmpText != null && !string.IsNullOrEmpty(tmpText.text))
                {
                    return CleanText(tmpText.text);
                }

                legacyText = obj.transform.parent.GetComponentInChildren<Text>(true);
                if (legacyText != null && !string.IsNullOrEmpty(legacyText.text))
                {
                    return CleanText(legacyText.text);
                }
            }

            return obj.name;
        }

        /// <summary>
        /// Check if menu is currently active.
        /// </summary>
        public static bool IsMenuActive()
        {
            return _menuActive;
        }
    }
}
