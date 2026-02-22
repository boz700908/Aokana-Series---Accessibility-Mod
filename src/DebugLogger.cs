using System;
using MelonLoader;

namespace AokanaAccess
{
    /// <summary>
    /// Debug logging utility. Only active when debug mode is enabled.
    /// </summary>
    public static class DebugLogger
    {
        private static bool _debugMode = false;

        /// <summary>
        /// Enable or disable debug logging.
        /// </summary>
        public static bool DebugMode
        {
            get => _debugMode;
            set => _debugMode = value;
        }

        /// <summary>
        /// Log a message if debug mode is enabled.
        /// </summary>
        public static void Log(string message)
        {
            if (_debugMode)
            {
                MelonLogger.Msg($"[AokanaAccess] {message}");
            }
        }

        /// <summary>
        /// Log an error message (always logged, regardless of debug mode).
        /// </summary>
        public static void LogError(string message)
        {
            MelonLogger.Error($"[AokanaAccess] {message}");
        }
    }
}
