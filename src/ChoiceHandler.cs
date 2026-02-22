using System.Collections.Generic;
using UnityEngine;

namespace AokanaAccess
{
    /// <summary>
    /// Handles choice menu announcements and navigation.
    /// </summary>
    public static class ChoiceHandler
    {
        private static List<string> _currentChoices = new List<string>();
        private static int _currentChoiceIndex = -1;
        private static bool _choicesActive = false;

        /// <summary>
        /// Called when a choice menu is displayed.
        /// This is invoked by the Harmony patch on AdvChoice.ShowMenu.
        /// </summary>
        public static void OnChoicesShown(List<string> choices)
        {
            _currentChoices = new List<string>(choices);
            _currentChoiceIndex = 0;
            _choicesActive = true;

            DebugLogger.Log($"Choices shown: {choices.Count} options");

            // Announce all choices
            AnnounceAllChoices();
        }

        /// <summary>
        /// Called when the choice menu is hidden.
        /// </summary>
        public static void OnChoicesHidden()
        {
            _choicesActive = false;
            _currentChoices.Clear();
            _currentChoiceIndex = -1;

            DebugLogger.Log("Choices hidden");
        }

        /// <summary>
        /// Announce all available choices.
        /// </summary>
        private static void AnnounceAllChoices()
        {
            if (_currentChoices.Count == 0)
            {
                return;
            }

            // Just read the choices without extra text
            string announcement = string.Join(". ", _currentChoices.ToArray());

            ScreenReader.Speak(announcement, interrupt: true);
            DebugLogger.Log($"Announced all choices: {announcement}");
        }

        /// <summary>
        /// Announce the currently selected choice.
        /// </summary>
        private static void AnnounceCurrentChoice()
        {
            if (!_choicesActive || _currentChoiceIndex < 0 || _currentChoiceIndex >= _currentChoices.Count)
            {
                return;
            }

            // Just read the choice text without extra information
            string announcement = _currentChoices[_currentChoiceIndex];

            ScreenReader.Speak(announcement, interrupt: true);
            DebugLogger.Log($"Announced choice {_currentChoiceIndex + 1}: {_currentChoices[_currentChoiceIndex]}");
        }

        /// <summary>
        /// Handle input for choice navigation.
        /// Called when arrow keys are pressed.
        /// </summary>
        public static void HandleInput()
        {
            if (!_choicesActive)
            {
                return;
            }

            // Up arrow - previous choice
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                if (_currentChoiceIndex > 0)
                {
                    _currentChoiceIndex--;
                }
                else
                {
                    _currentChoiceIndex = _currentChoices.Count - 1; // Wrap to last
                }
                AnnounceCurrentChoice();
            }

            // Down arrow - next choice
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                if (_currentChoiceIndex < _currentChoices.Count - 1)
                {
                    _currentChoiceIndex++;
                }
                else
                {
                    _currentChoiceIndex = 0; // Wrap to first
                }
                AnnounceCurrentChoice();
            }

            // F3 - Repeat all choices
            if (Input.GetKeyDown(KeyCode.F3))
            {
                AnnounceAllChoices();
            }

            // F4 - Repeat current choice
            if (Input.GetKeyDown(KeyCode.F4))
            {
                AnnounceCurrentChoice();
            }
        }

        /// <summary>
        /// Check if choices are currently active.
        /// </summary>
        public static bool AreChoicesActive()
        {
            return _choicesActive;
        }
    }
}
