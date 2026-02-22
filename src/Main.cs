using MelonLoader;
using UnityEngine;
using HarmonyLib;

[assembly: MelonInfo(typeof(AokanaAccess.Main), "Aokana Accessibility Mod", "1.0.0", "AccessibilityMod")]
// Support main game and EXTRA1 - MelonLoader will load this mod for both games
// [assembly: MelonGame("NekoNyanSoft", "Aokana")] - Removed to support EXTRA1

namespace AokanaAccess
{
    /// <summary>
    /// Main entry point for the Aokana Accessibility Mod.
    /// Supports: Aokana - Four Rhythms Across the Blue and EXTRA1.
    /// </summary>
    public class Main : MelonMod
    {
        private static string _gameName = "Unknown";

        public override void OnInitializeMelon()
        {
            // Detect which game we're running in
            DetectGame();

            // Initialize localization config system
            LocalizationConfig.Initialize();

            // Initialize localization
            Loc.Initialize();

            // Initialize screen reader
            ScreenReader.Initialize();

            // Apply Harmony patches
            MelonLogger.Msg($"Applying Harmony patches for {_gameName}...");
            var harmony = new HarmonyLib.Harmony("com.accessibilitymod.aokana");
            harmony.PatchAll();
            MelonLogger.Msg("Harmony patches applied successfully");

            MelonLogger.Msg($"Aokana Accessibility Mod initialized successfully for {_gameName}");
        }

        private void DetectGame()
        {
            string exeName = System.Diagnostics.Process.GetCurrentProcess().ProcessName;

            if (exeName.Contains("EXTRA1"))
            {
                _gameName = "Aokana EXTRA1";
            }
            else
            {
                _gameName = "Aokana - Four Rhythms Across the Blue";
            }

            MelonLogger.Msg($"Detected game: {_gameName}");
        }

        public override void OnUpdate()
        {
            // Handle dialog-related input
            DialogHandler.HandleInput();

            // Handle choice-related input
            ChoiceHandler.HandleInput();

            // Handle backlog-related input
            BacklogHandler.HandleInput();

            // Handle menu-related input
            MenuHandler.HandleInput();

            // Handle quick access features (quick save/load, voice bookmarks)
            QuickAccessHandler.HandleInput();

            // Handle settings menu shortcuts
            SettingsHandler.HandleInput();

            // Handle slot save/load shortcuts
            SlotSaveHandler.HandleInput();

            // Toggle debug mode with F12
            if (Input.GetKeyDown(KeyCode.F12))
            {
                DebugLogger.DebugMode = !DebugLogger.DebugMode;
                string message = DebugLogger.DebugMode ?
                    Loc.Get("debug_enabled") :
                    Loc.Get("debug_disabled");
                ScreenReader.Speak(message);
            }

            // Export menus and dialogs with Alt+Shift+E (debug mode only)
            if (DebugLogger.DebugMode &&
                (Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt)) &&
                (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) &&
                Input.GetKeyDown(KeyCode.E))
            {
                MelonLogger.Msg("[Main] Starting debug export...");
                ScreenReader.Speak("Starting export", interrupt: true);
                DebugExporter.ExportAllMenusAndDialogs();
            }
        }

        public override void OnApplicationQuit()
        {
            // Don't shutdown ScreenReader here - it's called too early when Alt+F4 is pressed
            // ScreenReader will be shut down by the QuitGame patch instead
        }
    }
}
