using UnityEngine;

namespace AokanaAccess
{
    /// <summary>
    /// Handles dialog text and character name announcements.
    /// </summary>
    public static class DialogHandler
    {
        private static string _lastDialogText = string.Empty;
        private static string _lastCharacterName = string.Empty;
        private static bool _autoReadEnabled = true;

        // Track the last announced text to prevent duplicate announcements
        private static string _lastAnnouncedText = string.Empty;

        /// <summary>
        /// Enable or disable automatic reading of new dialog.
        /// </summary>
        public static bool AutoReadEnabled
        {
            get => _autoReadEnabled;
            set
            {
                _autoReadEnabled = value;
                string message = value ?
                    Loc.Get("autoread_enabled") :
                    Loc.Get("autoread_disabled");
                ScreenReader.Speak(message);
            }
        }

        /// <summary>
        /// Called when new dialog text is displayed.
        /// This is invoked by the Harmony patch on UIAdv.ShowText.
        /// </summary>
        public static void OnDialogShown(string characterName, string dialogText)
        {
            // Store for repeat functionality
            _lastCharacterName = characterName ?? string.Empty;
            _lastDialogText = dialogText ?? string.Empty;

            DebugLogger.Log($"Dialog shown - Name: '{_lastCharacterName}', Text: '{_lastDialogText}'");

            // Announce if auto-read is enabled
            if (_autoReadEnabled)
            {
                AnnounceDialog();
            }
        }

        /// <summary>
        /// Announce the current dialog with character name.
        /// </summary>
        private static void AnnounceDialog()
        {
            if (string.IsNullOrEmpty(_lastDialogText))
            {
                return;
            }

            string announcement;
            if (!string.IsNullOrEmpty(_lastCharacterName))
            {
                // Format: "Character name: dialog text"
                announcement = $"{_lastCharacterName}: {_lastDialogText}";
            }
            else
            {
                // No character name, just the dialog
                announcement = _lastDialogText;
            }

            // Only announce if the text has actually changed
            if (announcement != _lastAnnouncedText)
            {
                _lastAnnouncedText = announcement;
                ScreenReader.Speak(announcement, interrupt: true);
                DebugLogger.Log($"Announced: '{announcement}'");
            }
            else
            {
                DebugLogger.Log("Skipped duplicate announcement");
            }
        }

        /// <summary>
        /// Repeat the last dialog text (F1 key).
        /// </summary>
        public static void RepeatLastDialog()
        {
            if (string.IsNullOrEmpty(_lastDialogText))
            {
                // No dialog to repeat - do nothing
                return;
            }

            ScreenReader.Speak(_lastDialogText, interrupt: true);
        }

        /// <summary>
        /// Repeat the last character name (F2 key).
        /// </summary>
        public static void RepeatCharacterName()
        {
            if (string.IsNullOrEmpty(_lastCharacterName))
            {
                // No character name - do nothing
                return;
            }

            ScreenReader.Speak(_lastCharacterName, interrupt: true);
        }

        /// <summary>
        /// Handle input for dialog-related keys.
        /// </summary>
        public static void HandleInput()
        {
            // F1 - Repeat last dialog
            if (Input.GetKeyDown(KeyCode.F1))
            {
                RepeatLastDialog();
            }

            // F2 - Repeat character name
            if (Input.GetKeyDown(KeyCode.F2))
            {
                RepeatCharacterName();
            }

            // F6 - Toggle auto-read
            if (Input.GetKeyDown(KeyCode.F6))
            {
                AutoReadEnabled = !AutoReadEnabled;
            }
        }
    }
}
