using UnityEngine;

namespace AokanaAccess
{
    /// <summary>
    /// Handles quick save/load and voice bookmark accessibility features.
    /// </summary>
    public static class QuickAccessHandler
    {
        private static EngineMain _engine = null;

        /// <summary>
        /// Initialize the handler with engine reference.
        /// </summary>
        public static void Initialize(EngineMain engine)
        {
            _engine = engine;
            DebugLogger.Log("QuickAccessHandler initialized");
        }

        /// <summary>
        /// Handle input for quick access features.
        /// </summary>
        public static void HandleInput()
        {
            if (_engine == null)
            {
                return;
            }

            // F8 - Quick Save
            if (Input.GetKeyDown(KeyCode.F8))
            {
                QuickSave();
            }

            // F9 - Quick Load
            if (Input.GetKeyDown(KeyCode.F9))
            {
                QuickLoad();
            }

            // F10 - Quick Voice Bookmark
            if (Input.GetKeyDown(KeyCode.F10))
            {
                QuickVoiceBookmark();
            }

            // F11 - Open Voice Bookmarks Menu
            if (Input.GetKeyDown(KeyCode.F11))
            {
                OpenVoiceBookmarks();
            }
        }

        /// <summary>
        /// Perform quick save.
        /// </summary>
        private static void QuickSave()
        {
            try
            {
                // Only allow quick save in game mode
                if (_engine.uiflow.mode != UIFlow.RunState.game)
                {
                    DebugLogger.Log("Quick save not available - not in game mode");
                    return;
                }

                DebugLogger.Log("Quick save triggered");
                _engine.QuickSave();
                ScreenReader.Speak(Loc.Get("quick_save_done"), interrupt: true);
            }
            catch (System.Exception ex)
            {
                DebugLogger.Log($"Quick save failed: {ex.Message}");
                ScreenReader.Speak(Loc.Get("quick_save_failed"), interrupt: true);
            }
        }

        /// <summary>
        /// Perform quick load (with confirmation).
        /// </summary>
        private static void QuickLoad()
        {
            try
            {
                // Only allow quick load in game mode
                if (_engine.uiflow.mode != UIFlow.RunState.game)
                {
                    DebugLogger.Log("Quick load not available - not in game mode");
                    return;
                }

                DebugLogger.Log("Quick load triggered");
                _engine.QuickLoad();
                // Note: This will show confirmation dialog, which MenuHandler will announce
            }
            catch (System.Exception ex)
            {
                DebugLogger.Log($"Quick load failed: {ex.Message}");
                ScreenReader.Speak(Loc.Get("quick_load_failed"), interrupt: true);
            }
        }

        /// <summary>
        /// Create quick voice bookmark.
        /// </summary>
        private static void QuickVoiceBookmark()
        {
            try
            {
                // Only allow voice bookmark in game mode
                if (_engine.uiflow.mode != UIFlow.RunState.game)
                {
                    DebugLogger.Log("Quick voice bookmark not available - not in game mode");
                    return;
                }

                DebugLogger.Log("Quick voice bookmark triggered");
                _engine.QuickVBookmark();
                ScreenReader.Speak(Loc.Get("voice_bookmark_saved"), interrupt: true);
            }
            catch (System.Exception ex)
            {
                DebugLogger.Log($"Quick voice bookmark failed: {ex.Message}");
                ScreenReader.Speak(Loc.Get("voice_bookmark_failed"), interrupt: true);
            }
        }

        /// <summary>
        /// Open voice bookmarks menu.
        /// </summary>
        private static void OpenVoiceBookmarks()
        {
            try
            {
                DebugLogger.Log("Opening voice bookmarks menu");
                _engine.uiflow.OpenVoiceBookmarks();
                // MenuHandler will announce when menu opens
            }
            catch (System.Exception ex)
            {
                DebugLogger.Log($"Failed to open voice bookmarks: {ex.Message}");
                ScreenReader.Speak(Loc.Get("menu_open_failed"), interrupt: true);
            }
        }
    }
}
