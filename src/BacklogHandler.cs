using System.Collections.Generic;

namespace AokanaAccess
{
    /// <summary>
    /// Handles backlog announcements and navigation.
    /// </summary>
    public static class BacklogHandler
    {
        private static bool _backlogActive = false;
        private static List<BacklogEntry> _currentEntries = null;
        private static int _currentStartIndex = -1;
        private static bool _justOpened = false;

        /// <summary>
        /// Called when the backlog is shown.
        /// </summary>
        public static void OnBacklogShown(Backlog backlog)
        {
            _backlogActive = true;
            _currentEntries = backlog.entries;
            _currentStartIndex = -1;
            _justOpened = true;

            DebugLogger.Log($"Backlog shown with {_currentEntries.Count} entries");
        }

        /// <summary>
        /// Called when the backlog is hidden.
        /// </summary>
        public static void OnBacklogHidden()
        {
            if (!_backlogActive)
            {
                // Already closed, don't announce again
                return;
            }

            _backlogActive = false;
            _currentEntries = null;
            _currentStartIndex = -1;

            DebugLogger.Log("Backlog hidden");

            // Announce backlog closed
            ScreenReader.Speak(Loc.Get("backlog_closed"), interrupt: true);
        }

        /// <summary>
        /// Called when the backlog scrolls to a new position.
        /// </summary>
        public static void OnBacklogScrolled(int startIndex)
        {
            if (!_backlogActive || _currentEntries == null || startIndex == _currentStartIndex)
            {
                return;
            }

            _currentStartIndex = startIndex;

            // Announce the topmost visible entry
            if (startIndex >= 0 && startIndex < _currentEntries.Count)
            {
                BacklogEntry entry = _currentEntries[startIndex];
                string charName = ExtractCurrentLanguageText(entry.scr.charname);
                string text = ExtractCurrentLanguageText(entry.scr.text);

                string announcement;
                if (!string.IsNullOrEmpty(charName))
                {
                    announcement = $"{charName}: {text}";
                }
                else
                {
                    announcement = text;
                }

                // Add status messages
                if (_justOpened)
                {
                    announcement = Loc.Get("backlog_opened") + ". " + announcement;
                    _justOpened = false;
                }
                else
                {
                    // Check if at beginning or end (only when not just opened)
                    bool atBeginning = (startIndex == 0);
                    int maxIndex = _currentEntries.Count - 4;
                    if (maxIndex < 0) maxIndex = 0;
                    bool atEnd = (startIndex >= maxIndex && _currentEntries.Count > 4);

                    if (atBeginning && !atEnd)
                    {
                        announcement = Loc.Get("backlog_beginning") + ". " + announcement;
                    }
                    else if (atEnd && !atBeginning)
                    {
                        announcement = Loc.Get("backlog_end") + ". " + announcement;
                    }
                }

                ScreenReader.Speak(announcement, interrupt: true);
                DebugLogger.Log($"Announced backlog entry {startIndex}: {announcement}");
            }
        }

        /// <summary>
        /// Handle input for backlog features.
        /// </summary>
        public static void HandleInput()
        {
            if (!_backlogActive)
            {
                return;
            }

            // F5 - Read all visible entries
            if (UnityEngine.Input.GetKeyDown(UnityEngine.KeyCode.F5))
            {
                AnnounceVisibleEntries();
            }
        }

        /// <summary>
        /// Announce all currently visible backlog entries.
        /// </summary>
        private static void AnnounceVisibleEntries()
        {
            if (_currentEntries == null || _currentStartIndex < 0)
            {
                return;
            }

            List<string> announcements = new List<string>();
            int maxVisible = 4; // UIBacklog shows 4 entries at a time

            for (int i = 0; i < maxVisible && (_currentStartIndex + i) < _currentEntries.Count; i++)
            {
                BacklogEntry entry = _currentEntries[_currentStartIndex + i];
                string charName = ExtractCurrentLanguageText(entry.scr.charname);
                string text = ExtractCurrentLanguageText(entry.scr.text);

                if (!string.IsNullOrEmpty(charName))
                {
                    announcements.Add($"{charName}: {text}");
                }
                else
                {
                    announcements.Add(text);
                }
            }

            if (announcements.Count > 0)
            {
                string fullAnnouncement = string.Join(". ", announcements.ToArray());
                ScreenReader.Speak(fullAnnouncement, interrupt: true);
                DebugLogger.Log($"Announced {announcements.Count} visible entries");
            }
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

        /// <summary>
        /// Check if backlog is currently active.
        /// </summary>
        public static bool IsBacklogActive()
        {
            return _backlogActive;
        }
    }
}
