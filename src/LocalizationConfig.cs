using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace AokanaAccess
{
    /// <summary>
    /// Manages localization configuration files stored in JSON format.
    /// Uses simple key-value dictionary for .NET 3.5 compatibility.
    /// </summary>
    public static class LocalizationConfig
    {
        private static string _configBasePath;
        private static string _currentLanguage = "en";
        private static Dictionary<string, Dictionary<string, string>> _loadedConfigs = new Dictionary<string, Dictionary<string, string>>();
        private static bool _initialized = false;

        // Config file names
        private const string MENUS_CONFIG = "menus.json";
        private const string DIALOGS_CONFIG = "dialogs.json";
        private const string MESSAGES_CONFIG = "messages.json";
        private const string UI_ELEMENTS_CONFIG = "ui_elements.json";

        /// <summary>
        /// Initialize the localization config system.
        /// </summary>
        public static void Initialize()
        {
            try
            {
                // Get game directory
                string gameDirectory = Directory.GetCurrentDirectory();

                // Set config base path: [Game Directory]/UserData/AokanaAccess/
                _configBasePath = Path.Combine(Path.Combine(gameDirectory, "UserData"), "AokanaAccess");

                MelonLoader.MelonLogger.Msg($"[LocalizationConfig] Config base path: {_configBasePath}");

                // Create directory structure if it doesn't exist
                CreateDirectoryStructure();

                _initialized = true;
                MelonLoader.MelonLogger.Msg("[LocalizationConfig] Initialized successfully");
            }
            catch (Exception ex)
            {
                MelonLoader.MelonLogger.Error($"[LocalizationConfig] Initialization error: {ex.Message}");
            }
        }

        /// <summary>
        /// Set the current language and reload configs.
        /// </summary>
        public static void SetLanguage(string language)
        {
            if (_currentLanguage == language)
            {
                return;
            }

            _currentLanguage = language;
            _loadedConfigs.Clear();

            MelonLoader.MelonLogger.Msg($"[LocalizationConfig] Language changed to: {language}");
        }

        /// <summary>
        /// Get a localized string from the config files.
        /// </summary>
        /// <param name="configFile">Config file name (menus, dialogs, messages, ui_elements)</param>
        /// <param name="key">Flat key (e.g., "title_menu.menu_name" or "mod.loaded")</param>
        /// <param name="fallback">Fallback string if not found</param>
        /// <returns>Localized string or fallback</returns>
        public static string Get(string configFile, string key, string fallback = "")
        {
            if (!_initialized)
            {
                return fallback;
            }

            try
            {
                // Load config if not already loaded
                string configKey = $"{_currentLanguage}_{configFile}";
                if (!_loadedConfigs.ContainsKey(configKey))
                {
                    LoadConfig(configFile);
                }

                // Get value from dictionary
                if (_loadedConfigs.TryGetValue(configKey, out Dictionary<string, string> config))
                {
                    if (config.TryGetValue(key, out string value))
                    {
                        return value;
                    }
                }

                DebugLogger.Log($"[LocalizationConfig] Key not found: {configFile}.{key}, using fallback: {fallback}");
                return fallback;
            }
            catch (Exception ex)
            {
                MelonLoader.MelonLogger.Error($"[LocalizationConfig] Error getting '{key}' from {configFile}: {ex.Message}");
                return fallback;
            }
        }

        /// <summary>
        /// Get a localized string with format parameters.
        /// </summary>
        public static string GetFormatted(string configFile, string key, string fallback, params object[] args)
        {
            string template = Get(configFile, key, fallback);
            try
            {
                return string.Format(template, args);
            }
            catch (Exception ex)
            {
                MelonLoader.MelonLogger.Error($"[LocalizationConfig] Format error for '{key}': {ex.Message}");
                return template;
            }
        }

        /// <summary>
        /// Load a config file for the current language.
        /// Uses simple key-value format for .NET 3.5 compatibility.
        /// </summary>
        private static void LoadConfig(string configFile)
        {
            string configKey = $"{_currentLanguage}_{configFile}";
            string configPath = Path.Combine(Path.Combine(_configBasePath, _currentLanguage), configFile);

            try
            {
                if (File.Exists(configPath))
                {
                    string json = File.ReadAllText(configPath);

                    // Parse simple key-value JSON format
                    Dictionary<string, string> config = ParseSimpleJson(json);
                    _loadedConfigs[configKey] = config;

                    DebugLogger.Log($"[LocalizationConfig] Loaded config: {configPath} ({config.Count} keys)");
                }
                else
                {
                    DebugLogger.Log($"[LocalizationConfig] Config file not found: {configPath}");
                    _loadedConfigs[configKey] = new Dictionary<string, string>(); // Empty config
                }
            }
            catch (Exception ex)
            {
                MelonLoader.MelonLogger.Error($"[LocalizationConfig] Error loading {configPath}: {ex.Message}");
                _loadedConfigs[configKey] = new Dictionary<string, string>(); // Empty config on error
            }
        }

        /// <summary>
        /// Parse simple flat JSON format into key-value dictionary.
        /// Format: { "key1": "value1", "key2": "value2" }
        /// </summary>
        private static Dictionary<string, string> ParseSimpleJson(string json)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();

            try
            {
                // Remove outer braces and whitespace
                json = json.Trim();
                if (json.StartsWith("{"))
                {
                    json = json.Substring(1);
                }
                if (json.EndsWith("}"))
                {
                    json = json.Substring(0, json.Length - 1);
                }

                // Split by comma (simple parser, doesn't handle nested objects)
                string[] pairs = json.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                foreach (string pair in pairs)
                {
                    // Split by colon
                    int colonIndex = pair.IndexOf(':');
                    if (colonIndex > 0)
                    {
                        string key = pair.Substring(0, colonIndex).Trim().Trim('"');
                        string value = pair.Substring(colonIndex + 1).Trim().Trim('"');

                        // Unescape common JSON escapes
                        value = value.Replace("\\n", "\n").Replace("\\\"", "\"");

                        result[key] = value;
                    }
                }
            }
            catch (Exception ex)
            {
                MelonLoader.MelonLogger.Error($"[LocalizationConfig] JSON parse error: {ex.Message}");
            }

            return result;
        }

        /// <summary>
        /// Create directory structure for all supported languages.
        /// </summary>
        private static void CreateDirectoryStructure()
        {
            string[] languages = { "en", "cn", "tc", "jp" };

            foreach (string lang in languages)
            {
                string langPath = Path.Combine(_configBasePath, lang);
                if (!Directory.Exists(langPath))
                {
                    Directory.CreateDirectory(langPath);
                    MelonLoader.MelonLogger.Msg($"[LocalizationConfig] Created directory: {langPath}");
                }
            }
        }

        /// <summary>
        /// Reload all configs (useful for hot-reload in debug mode).
        /// </summary>
        public static void ReloadConfigs()
        {
            _loadedConfigs.Clear();
            MelonLoader.MelonLogger.Msg("[LocalizationConfig] All configs reloaded");
        }

        /// <summary>
        /// Get the config base path.
        /// </summary>
        public static string GetConfigBasePath()
        {
            return _configBasePath;
        }

        /// <summary>
        /// Check if a config file exists for the current language.
        /// </summary>
        public static bool ConfigExists(string configFile)
        {
            string configPath = Path.Combine(Path.Combine(_configBasePath, _currentLanguage), configFile);
            return File.Exists(configPath);
        }
    }
}
