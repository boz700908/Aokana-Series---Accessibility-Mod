using System;
using System.Runtime.InteropServices;

namespace AokanaAccess
{
    /// <summary>
    /// Wrapper for Tolk screen reader library.
    /// Provides text-to-speech output through the user's active screen reader.
    /// </summary>
    public static class ScreenReader
    {
        private static bool _isInitialized = false;

        // Tolk DLL imports
        [DllImport("Tolk.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void Tolk_Load();

        [DllImport("Tolk.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void Tolk_Unload();

        [DllImport("Tolk.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern bool Tolk_IsLoaded();

        [DllImport("Tolk.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        private static extern void Tolk_Output(string text, bool interrupt);

        [DllImport("Tolk.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        private static extern bool Tolk_Speak(string text, bool interrupt);

        [DllImport("Tolk.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void Tolk_Silence();

        /// <summary>
        /// Initialize the screen reader library.
        /// Must be called before any other ScreenReader methods.
        /// </summary>
        public static void Initialize()
        {
            if (_isInitialized)
            {
                MelonLoader.MelonLogger.Msg("[AokanaAccess] ScreenReader already initialized");
                return;
            }

            try
            {
                MelonLoader.MelonLogger.Msg("[AokanaAccess] Initializing ScreenReader...");
                Tolk_Load();
                _isInitialized = Tolk_IsLoaded();

                if (_isInitialized)
                {
                    MelonLoader.MelonLogger.Msg("[AokanaAccess] ScreenReader initialized successfully");
                }
                else
                {
                    MelonLoader.MelonLogger.Msg("[AokanaAccess] Tolk failed to load. Screen reader may not be running.");
                }
            }
            catch (Exception ex)
            {
                MelonLoader.MelonLogger.Error($"[AokanaAccess] Failed to initialize Tolk: {ex.Message}");
            }
        }

        /// <summary>
        /// Shut down the screen reader library.
        /// </summary>
        public static void Shutdown()
        {
            if (!_isInitialized)
                return;

            try
            {
                MelonLoader.MelonLogger.Msg("[AokanaAccess] Shutting down ScreenReader...");
                Tolk_Unload();
                _isInitialized = false;
                MelonLoader.MelonLogger.Msg("[AokanaAccess] ScreenReader shut down");
            }
            catch (Exception ex)
            {
                MelonLoader.MelonLogger.Error($"[AokanaAccess] Error during Tolk shutdown: {ex.Message}");
            }
        }

        /// <summary>
        /// Speak text through the screen reader.
        /// </summary>
        /// <param name="text">Text to speak</param>
        /// <param name="interrupt">If true, interrupts currently speaking text</param>
        public static void Speak(string text, bool interrupt = true)
        {
            if (string.IsNullOrEmpty(text))
                return;

            if (!_isInitialized)
            {
                MelonLoader.MelonLogger.Msg($"[AokanaAccess] ScreenReader not initialized (_isInitialized={_isInitialized}). Would speak: {text}");
                // Try to re-check if Tolk is actually loaded
                try
                {
                    bool actuallyLoaded = Tolk_IsLoaded();
                    MelonLoader.MelonLogger.Msg($"[AokanaAccess] Tolk_IsLoaded() returns: {actuallyLoaded}");
                    if (actuallyLoaded)
                    {
                        MelonLoader.MelonLogger.Msg("[AokanaAccess] Tolk is loaded but _isInitialized is false! Fixing...");
                        _isInitialized = true;
                        // Try speaking again
                        Tolk_Speak(text, interrupt);
                        DebugLogger.Log($"Spoke: {text}");
                        return;
                    }
                }
                catch (Exception ex)
                {
                    MelonLoader.MelonLogger.Error($"[AokanaAccess] Error checking Tolk status: {ex.Message}");
                }
                return;
            }

            try
            {
                Tolk_Speak(text, interrupt);
                DebugLogger.Log($"Spoke: {text}");
            }
            catch (Exception ex)
            {
                MelonLoader.MelonLogger.Error($"[AokanaAccess] Error speaking text: {ex.Message}");
            }
        }

        /// <summary>
        /// Stop all currently speaking text.
        /// </summary>
        public static void Silence()
        {
            if (!_isInitialized)
                return;

            try
            {
                Tolk_Silence();
            }
            catch (Exception ex)
            {
                DebugLogger.Log($"Error silencing speech: {ex.Message}");
            }
        }
    }
}
