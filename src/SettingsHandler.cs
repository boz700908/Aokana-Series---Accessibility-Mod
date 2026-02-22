using UnityEngine;

namespace AokanaAccess
{
    /// <summary>
    /// Handles settings menu accessibility features.
    /// </summary>
    public static class SettingsHandler
    {
        private static bool _isActive = false;
        private static int _currentTab = 0;
        private static string _lastAnnouncement = string.Empty;
        private static EngineMain _engine = null;
        private static float _lastTabChangeTime = 0f;
        private static int _lastTabIndex = -1;

        /// <summary>
        /// Initialize the handler with engine reference.
        /// </summary>
        public static void Initialize(EngineMain engine)
        {
            _engine = engine;
            DebugLogger.Log("SettingsHandler initialized");
        }

        /// <summary>
        /// Handle input for settings menu shortcuts.
        /// </summary>
        public static void HandleInput()
        {
            if (_engine == null)
            {
                return;
            }

            // Alt+O - Open settings menu
            if ((Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt)) && Input.GetKeyDown(KeyCode.O))
            {
                MelonLoader.MelonLogger.Msg("[SettingsHandler] Alt+O pressed - opening settings menu");
                _engine.uiflow.OpenOptionsGeneral(false);
            }
        }

        /// <summary>
        /// Called when settings menu is opened.
        /// </summary>
        public static void OnMenuOpened()
        {
            _isActive = true;
            _currentTab = 0;
            _lastAnnouncement = string.Empty;

            DebugLogger.Log("Settings menu opened");
            string announcement = Loc.Get("settings_opened");
            ScreenReader.Speak(announcement, interrupt: true);

            // Activate MenuHandler for focus tracking
            MenuHandler.OnMenuOpened(announcement);
        }

        /// <summary>
        /// Called when a tab is changed.
        /// </summary>
        public static void OnTabChanged(string tabName, int tabIndex)
        {
            // Debounce: ignore if same tab within 0.1 seconds
            if (tabIndex == _lastTabIndex && Time.time - _lastTabChangeTime < 0.1f)
            {
                DebugLogger.Log($"Debounced duplicate tab change to: {tabName} (index: {tabIndex})");
                return;
            }

            _lastTabIndex = tabIndex;
            _lastTabChangeTime = Time.time;
            _currentTab = tabIndex;
            _lastAnnouncement = tabName;

            DebugLogger.Log($"Settings tab changed to: {tabName} (index: {tabIndex})");
            ScreenReader.Speak(tabName, interrupt: true);
        }

        /// <summary>
        /// Called when menu is closed.
        /// </summary>
        public static void OnMenuClosed()
        {
            _isActive = false;
            _currentTab = 0;
            _lastAnnouncement = string.Empty;

            DebugLogger.Log("Settings menu closed");
            // Don't call MenuHandler.OnMenuClosed() - parent menu may still be active
        }

        /// <summary>
        /// Check if settings menu is active.
        /// </summary>
        public static bool IsActive()
        {
            return _isActive;
        }

        /// <summary>
        /// Repeat last announcement.
        /// </summary>
        public static void RepeatLastAnnouncement()
        {
            if (!string.IsNullOrEmpty(_lastAnnouncement))
            {
                ScreenReader.Speak(_lastAnnouncement, interrupt: true);
            }
        }
    }
}
