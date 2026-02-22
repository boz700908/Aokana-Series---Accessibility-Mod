# Localization Files

This directory contains localization files for the Aokana Accessibility Mod.

## Directory Structure

```
AokanaAccess/
├── en/          # English
├── cn/          # Simplified Chinese
├── tc/          # Traditional Chinese
└── jp/          # Japanese
```

Each language folder should contain:
- `menus.json` - Menu button localizations
- `dialogs.json` - Dialog box localizations
- `messages.json` - Mod message localizations
- `ui_elements.json` - UI element localizations

## File Format

All JSON files use flat key-value format:

```json
{
  "MenuID.ButtonID": "Localized Text",
  "DialogID.ElementID": "Localized Text"
}
```

## Creating Localization Files

### Step 1: Export from Game

1. Launch the game
2. Press **F12** to enable debug mode
3. Press **Alt+Shift+E** to export current scene
4. Find exported data in `[Game Directory]/debug/{language}/menu_export.json`

### Step 2: Convert Export to Localization Format

The export file has nested structure:

```json
{
  "menus": [
    {
      "id": "TitleBtnGrp",
      "buttons": [
        {
          "id": "btnNew",
          "text": "New Game"
        }
      ]
    }
  ]
}
```

Convert to flat format for `menus.json`:

```json
{
  "TitleBtnGrp.btnNew": "New Game"
}
```

**Key format:** `"MenuID.ButtonID": "Your Translation"`

### Step 3: Example Files

See `en/*.json.example` files for reference format:
- `menus.json.example` - Menu localization example
- `dialogs.json.example` - Dialog localization example

Copy and rename (remove `.example`) to use.

## Usage in Code

The mod uses `LocalizationConfig.Get()` to retrieve localized strings:

```csharp
// Get menu button text
string text = LocalizationConfig.Get("menus", "TitleBtnGrp.btnNew", "New Game");

// Get dialog text
string text = LocalizationConfig.Get("dialogs", "quit_confirm.message", "Are you sure?");

// Get mod message
string text = LocalizationConfig.Get("messages", "mod.loaded", "Mod loaded");
```

## Notes

- Keys use dot notation: `MenuID.ElementID`
- If a key is not found, the fallback text is used
- Files are loaded automatically based on game language
- Hot reload is supported in debug mode
