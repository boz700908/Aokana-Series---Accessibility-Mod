using UnityEngine;

namespace AokanaAccess
{
    /// <summary>
    /// Handles direct save/load to specific slots without using menus.
    /// Shift+1-9: Save to slot 1-9
    /// Alt+1-9: Load from slot 1-9
    /// </summary>
    public static class SlotSaveHandler
    {
        private static EngineMain _engine = null;

        /// <summary>
        /// Initialize the handler with engine reference.
        /// </summary>
        public static void Initialize(EngineMain engine)
        {
            _engine = engine;
            MelonLoader.MelonLogger.Msg("[AokanaAccess] SlotSaveHandler initialized");
        }

        /// <summary>
        /// Handle input for slot save/load shortcuts.
        /// </summary>
        public static void HandleInput()
        {
            if (_engine == null)
            {
                return;
            }

            // Check if Shift or Alt is held
            bool shiftHeld = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
            bool altHeld = Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt);

            // Only process if exactly one modifier is held
            if (shiftHeld == altHeld)
            {
                return;
            }

            // Check number keys 1-9
            for (int i = 1; i <= 9; i++)
            {
                KeyCode key = KeyCode.Alpha1 + (i - 1);
                if (Input.GetKeyDown(key))
                {
                    if (shiftHeld)
                    {
                        SaveToSlot(i);
                    }
                    else if (altHeld)
                    {
                        LoadFromSlot(i);
                    }
                    break;
                }
            }
        }

        /// <summary>
        /// Save game to specified slot.
        /// </summary>
        private static void SaveToSlot(int slotNumber)
        {
            try
            {
                MelonLoader.MelonLogger.Msg($"[AokanaAccess] Attempting to save to slot {slotNumber}");

                // Check if we're in a valid state to save
                if (_engine.uiflow.mode != UIFlow.RunState.game)
                {
                    string errorMsg = Loc.Get("slot_save_invalid_state");
                    ScreenReader.Speak(errorMsg, interrupt: true);
                    MelonLoader.MelonLogger.Warning($"[AokanaAccess] Cannot save - not in game mode (current mode: {_engine.uiflow.mode})");
                    return;
                }

                // Save to slot (0-based index)
                _engine.savedata.SaveFile(slotNumber - 1);

                string successMsg = Loc.Get("slot_save_success", slotNumber);
                ScreenReader.Speak(successMsg, interrupt: true);
                MelonLoader.MelonLogger.Msg($"[AokanaAccess] Successfully saved to slot {slotNumber}");
            }
            catch (System.Exception ex)
            {
                string errorMsg = Loc.Get("slot_save_failed", slotNumber);
                ScreenReader.Speak(errorMsg, interrupt: true);
                MelonLoader.MelonLogger.Error($"[AokanaAccess] Failed to save to slot {slotNumber}: {ex.Message}\n{ex.StackTrace}");
            }
        }

        /// <summary>
        /// Load game from specified slot.
        /// </summary>
        private static void LoadFromSlot(int slotNumber)
        {
            try
            {
                MelonLoader.MelonLogger.Msg($"[AokanaAccess] Attempting to load from slot {slotNumber}");

                // Check if we're in a valid state to load
                // Only allow loading in game mode (same as quick load)
                if (_engine.uiflow.mode != UIFlow.RunState.game)
                {
                    string errorMsg = Loc.Get("slot_load_invalid_state");
                    ScreenReader.Speak(errorMsg, interrupt: true);
                    MelonLoader.MelonLogger.Warning($"[AokanaAccess] Cannot load - not in game mode (current mode: {_engine.uiflow.mode})");
                    return;
                }

                // Load from slot (0-based index)
                // The game will automatically restart after loading
                _engine.savedata.LoadFile(slotNumber - 1);

                string successMsg = Loc.Get("slot_load_success", slotNumber);
                ScreenReader.Speak(successMsg, interrupt: true);
                MelonLoader.MelonLogger.Msg($"[AokanaAccess] Successfully loaded from slot {slotNumber}");
            }
            catch (System.Exception ex)
            {
                string errorMsg = Loc.Get("slot_load_failed", slotNumber);
                ScreenReader.Speak(errorMsg, interrupt: true);
                MelonLoader.MelonLogger.Error($"[AokanaAccess] Failed to load from slot {slotNumber}: {ex.Message}\n{ex.StackTrace}");
            }
        }
    }
}
