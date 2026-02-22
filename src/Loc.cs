using System.Collections.Generic;

namespace AokanaAccess
{
    /// <summary>
    /// Localization system for all user-facing text.
    /// Now uses LocalizationConfig to load strings from JSON files.
    /// Falls back to hardcoded English strings if JSON files are not available.
    /// </summary>
    public static class Loc
    {
        private static Dictionary<string, string> _fallbackStrings = new Dictionary<string, string>();
        private static string _currentLanguage = "en";
        private static bool _initialized = false;

        /// <summary>
        /// Get the current language code (en, cn, tc, jp).
        /// </summary>
        public static string CurrentLanguage
        {
            get { return _currentLanguage; }
        }

        /// <summary>
        /// Initialize the localization system.
        /// Starts with English as default, will be updated when game language is detected.
        /// </summary>
        public static void Initialize()
        {
            // Start with English as default
            _currentLanguage = "en";
            LoadFallbackStrings();
            _initialized = true;
            MelonLoader.MelonLogger.Msg("[Loc] Initialized with English, will update when game language is detected");
        }

        /// <summary>
        /// Update language based on game settings.
        /// Called after EngineMain is initialized.
        /// </summary>
        public static void UpdateLanguage()
        {
            if (!_initialized)
            {
                Initialize();
                return;
            }

            DetectGameLanguage();
            LocalizationConfig.SetLanguage(_currentLanguage);
        }

        /// <summary>
        /// Detect the current game language and set MOD language accordingly.
        /// </summary>
        private static void DetectGameLanguage()
        {
            try
            {
                // Get game language from EngineMain
                var lang = EngineMain.lang;

                string oldLanguage = _currentLanguage;

                switch (lang)
                {
                    case EngineMain.Language.cn:
                        _currentLanguage = "cn";
                        break;
                    case EngineMain.Language.tc:
                        _currentLanguage = "tc";
                        break;
                    case EngineMain.Language.jp:
                        _currentLanguage = "jp";
                        break;
                    default:
                        _currentLanguage = "en";
                        break;
                }

                if (oldLanguage != _currentLanguage)
                {
                    MelonLoader.MelonLogger.Msg($"[Loc] Language changed from {oldLanguage} to {_currentLanguage} (game language: {lang})");
                }
            }
            catch (System.Exception ex)
            {
                MelonLoader.MelonLogger.Warning($"[Loc] Failed to detect game language: {ex.Message}, keeping current language: {_currentLanguage}");
            }
        }

        /// <summary>
        /// Get a localized string by key.
        /// First tries to load from JSON config files, then falls back to hardcoded strings.
        /// </summary>
        /// <param name="key">String key</param>
        /// <returns>Localized string, or the key itself if not found</returns>
        public static string Get(string key)
        {
            // Try to get from messages.json config file
            string value = LocalizationConfig.Get("messages.json", key, "");

            if (!string.IsNullOrEmpty(value))
            {
                return value;
            }

            // Fall back to hardcoded strings
            if (_fallbackStrings.TryGetValue(key, out string fallback))
            {
                return fallback;
            }

            DebugLogger.LogError($"Missing localization key: {key}");
            return key;
        }

        /// <summary>
        /// Get a localized string with format parameters.
        /// </summary>
        public static string Get(string key, params object[] args)
        {
            string format = Get(key);
            return string.Format(format, args);
        }

        /// <summary>
        /// Load fallback English strings (used when JSON files are not available).
        /// </summary>
        private static void LoadFallbackStrings()
        {
            _fallbackStrings.Clear();

            // Mod startup
            _fallbackStrings["mod_loaded"] = "Aokana Accessibility Mod loaded. Press F12 to toggle debug mode.";
            _fallbackStrings["debug_enabled"] = "Debug mode enabled";
            _fallbackStrings["debug_disabled"] = "Debug mode disabled";

            // Dialog features
            _fallbackStrings["no_dialog"] = "No dialog to repeat";
            _fallbackStrings["no_character"] = "No character name";
            _fallbackStrings["autoread_enabled"] = "Auto-read enabled";
            _fallbackStrings["autoread_disabled"] = "Auto-read disabled";

            // Choice menu
            _fallbackStrings["choice_menu"] = "Choice menu";
            _fallbackStrings["choice_format"] = "Choice {0}: {1}";
            _fallbackStrings["no_choices"] = "No choices available";

            // Backlog
            _fallbackStrings["backlog_opened"] = "Backlog opened";
            _fallbackStrings["backlog_closed"] = "Backlog closed";
            _fallbackStrings["backlog_at_start"] = "At beginning of backlog";
            _fallbackStrings["backlog_at_end"] = "At end of backlog";
            _fallbackStrings["backlog_entry"] = "{0}: {1}";
            _fallbackStrings["backlog_no_entries"] = "No backlog entries";

            // Menu navigation
            _fallbackStrings["menu_opened"] = "{0} opened";
            _fallbackStrings["menu_closed"] = "{0} closed";
            _fallbackStrings["menu_title"] = "Title Menu";
            _fallbackStrings["menu_extra"] = "Extra Menu";
            _fallbackStrings["menu_settings"] = "Settings Menu";
            _fallbackStrings["menu_voice_bookmarks"] = "Voice Bookmarks Menu";

            // Settings menu
            _fallbackStrings["settings_tab_visual"] = "Visual Settings";
            _fallbackStrings["settings_tab_text"] = "Text Settings";
            _fallbackStrings["settings_tab_sound"] = "Sound Settings";
            _fallbackStrings["settings_tab_voice"] = "Voice Settings";
            _fallbackStrings["settings_slider_format"] = "{0}: {1}";
            _fallbackStrings["settings_toggle_format"] = "{0}: {1}";

            // Quick access
            _fallbackStrings["quick_save_success"] = "Quick save successful";
            _fallbackStrings["quick_save_failed"] = "Quick save failed";
            _fallbackStrings["quick_load_success"] = "Quick load successful";
            _fallbackStrings["quick_load_failed"] = "Quick load failed";
            _fallbackStrings["quick_load_no_save"] = "No quick save found";

            // Voice bookmarks
            _fallbackStrings["voice_bookmark_saved"] = "Voice bookmark saved to slot {0}";
            _fallbackStrings["voice_bookmark_loaded"] = "Voice bookmark loaded from slot {0}";
            _fallbackStrings["voice_bookmark_empty"] = "Empty slot";
            _fallbackStrings["voice_bookmark_slot_format"] = "Slot {0}: {1}";

            // Language names
            _fallbackStrings["lang_cn"] = "简体中文";
            _fallbackStrings["lang_tc"] = "繁體中文";
            _fallbackStrings["lang_jp"] = "日本語";
            _fallbackStrings["lang_en"] = "English";

            // Confirmation dialogs
            _fallbackStrings["dialog_quit"] = "Are you sure you want to quit?";
            _fallbackStrings["dialog_quick_load"] = "Load quick save?";
            _fallbackStrings["dialog_return_title"] = "Return to title screen?";
            _fallbackStrings["dialog_overwrite"] = "Overwrite save data?";
            _fallbackStrings["dialog_generic"] = "Confirmation dialog";

            // UI elements
            _fallbackStrings["page_format"] = "Page {0}";
            _fallbackStrings["slot_empty"] = "Slot {0}: Empty";
            _fallbackStrings["slot_format"] = "Slot {0}: {1}";
            _fallbackStrings["toggle_on"] = "On";
            _fallbackStrings["toggle_off"] = "Off";

            // Slot save/load
            _fallbackStrings["slot_save_success"] = "Saved to slot {0}";
            _fallbackStrings["slot_save_failed"] = "Failed to save to slot {0}";
            _fallbackStrings["slot_load_success"] = "Loaded from slot {0}";
            _fallbackStrings["slot_load_failed"] = "Failed to load from slot {0}";
            _fallbackStrings["slot_save_invalid_state"] = "Cannot save in current state";
            _fallbackStrings["slot_load_invalid_state"] = "Cannot load in current state";
        }
    }
}
