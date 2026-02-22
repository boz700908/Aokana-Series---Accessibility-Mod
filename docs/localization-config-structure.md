# Localization Configuration Structure

## Directory Structure

```
[Game Directory]/UserData/AokanaAccess/
├── en/
│   ├── menus.json
│   ├── dialogs.json
│   ├── messages.json
│   └── ui_elements.json
├── cn/
│   ├── menus.json
│   ├── dialogs.json
│   ├── messages.json
│   └── ui_elements.json
├── tc/
│   ├── menus.json
│   ├── dialogs.json
│   ├── messages.json
│   └── ui_elements.json
└── jp/
    ├── menus.json
    ├── dialogs.json
    ├── messages.json
    └── ui_elements.json
```

## JSON File Formats

All JSON files use flat key-value format for simplicity and .NET 3.5 compatibility.
Keys use dot notation: `MenuID.ElementID` or `DialogID.ElementID`

### menus.json
Contains all menu-related localizations using flat key-value format.

**Format:** `"MenuID.ButtonID": "Localized Text"`

```json
{
  "TitleBtnGrp.btnNew": "New Game",
  "TitleBtnGrp.btnLoad": "Load",
  "TitleBtnGrp.btnExtra": "Extra",
  "TitleBtnGrp.btnConfig": "Config",
  "TitleBtnGrp.btnQuit": "Quit",
  "SysMenus.btnSave": "Save",
  "SysMenus.btnLoad": "Load",
  "SysMenus.btnBacklog": "Backlog",
  "SysMenus.btnAuto": "Auto",
  "SysMenus.btnSkip": "Skip",
  "SysMenus.btnConfig": "Config",
  "SysMenus.btnTitle": "Title",
  "UIConf.btnVisual": "Visual",
  "UIConf.btnText": "Text",
  "UIConf.btnSound": "Sound",
  "UIConf.btnVoice": "Voice",
  "UIConf.btnClose": "Close",
  "grp_visual.slider_brightness": "Brightness",
  "grp_visual.slider_contrast": "Contrast",
  "grp_text.slider_speed": "Text Speed",
  "grp_text.slider_auto_speed": "Auto Speed",
  "grp_sound.slider_bgm": "BGM Volume",
  "grp_sound.slider_se": "SE Volume",
  "grp_voice.slider_voice": "Voice Volume"
}
```

### dialogs.json
Contains all dialog/confirmation box localizations using flat key-value format.

**Format:** `"DialogID.ElementID": "Localized Text"`

```json
{
  "quit_confirm.title": "Quit Confirmation",
  "quit_confirm.message": "Are you sure you want to quit?",
  "quit_confirm.btnYes": "Yes",
  "quit_confirm.btnNo": "No",
  "load_confirm.title": "Load Confirmation",
  "load_confirm.message": "Load this save?",
  "load_confirm.btnYes": "Yes",
  "load_confirm.btnNo": "No",
  "overwrite_confirm.title": "Overwrite Confirmation",
  "overwrite_confirm.message": "Overwrite this save?",
  "overwrite_confirm.btnYes": "Yes",
  "overwrite_confirm.btnNo": "No",
  "title_confirm.title": "Return to Title",
  "title_confirm.message": "Return to title screen?",
  "title_confirm.btnYes": "Yes",
  "title_confirm.btnNo": "No"
}
```

### messages.json
Contains all mod messages and announcements using flat key-value format.

**Format:** `"category.message_id": "Localized Text"`

```json
{
  "mod.loaded": "Aokana Accessibility Mod loaded",
  "mod.debug_enabled": "Debug mode enabled",
  "mod.debug_disabled": "Debug mode disabled",
  "menu.opened": "{0} opened",
  "menu.closed": "{0} closed",
  "dialog.auto_read_on": "Auto-read enabled",
  "dialog.auto_read_off": "Auto-read disabled",
  "save.quick_save_success": "Quick save successful",
  "save.quick_load_success": "Quick load successful",
  "save.slot_save_success": "Saved to slot {0}",
  "save.slot_load_success": "Loaded from slot {0}",
  "backlog.opened": "Backlog opened",
  "backlog.closed": "Backlog closed",
  "backlog.at_beginning": "At beginning of backlog",
  "backlog.at_end": "At end of backlog"
}
```

### ui_elements.json
Contains UI element localizations using flat key-value format.

**Format:** `"category.element_id": "Localized Text"`

```json
{
  "common.ok": "OK",
  "common.cancel": "Cancel",
  "common.yes": "Yes",
  "common.no": "No",
  "common.close": "Close",
  "common.back": "Back",
  "navigation.next": "Next",
  "navigation.previous": "Previous",
  "navigation.page_up": "Page Up",
  "navigation.page_down": "Page Down"
}
```

## Debug Export Format

### menu_export.json
Exported by debug function (Alt+Shift+E in debug mode).
This file helps you create the localization JSON files by showing all menu IDs and their current text.

```json
{
  "export_info": {
    "timestamp": "2026-02-20T10:30:00",
    "game_version": "1.0.0",
    "language": "en"
  },
  "menus": [
    {
      "id": "TitleBtnGrp",
      "name": "Title Menu",
      "hierarchy": "Canvas/TitleBtnGrp",
      "buttons": [
        {
          "id": "btnNew",
          "text": "New Game",
          "image": "btnNew_sprite.png",
          "hierarchy": "Canvas/TitleBtnGrp/Title/btnNew"
        },
        {
          "id": "btnLoad",
          "text": "Load",
          "image": "btnLoad_sprite.png",
          "hierarchy": "Canvas/TitleBtnGrp/Title/btnLoad"
        }
      ]
    }
  ],
  "dialogs": [
    {
      "id": "UIConf",
      "name": "Confirmation Dialog",
      "hierarchy": "Canvas/UIConf",
      "message_field": "txtMessage",
      "buttons": [
        {
          "id": "btnYes",
          "text": "Yes",
          "image": "btnYes_sprite.png",
          "hierarchy": "Canvas/UIConf/btnYes"
        },
        {
          "id": "btnNo",
          "text": "No",
          "image": "btnNo_sprite.png",
          "hierarchy": "Canvas/UIConf/btnNo"
        }
      ]
    }
  ]
}
```

**How to use the export:**
1. Press F12 in game to enable debug mode
2. Press Alt+Shift+E to export current scene
3. Check `[Game Directory]/debug/{language}/menu_export.json`
4. Use the exported IDs to create your localization files:
   - For menus: `"MenuID.ButtonID": "Localized Text"`
   - For dialogs: `"DialogID.ElementID": "Localized Text"`
5. Images are exported to `debug/{language}/images/`

## Implementation Notes

1. **Flat Key-Value Format**: All JSON files use simple flat format `"key": "value"` for .NET 3.5 compatibility
2. **Key Format**: Use dot notation - `"MenuID.ElementID": "Text"` or `"category.message_id": "Text"`
3. **Fallback System**: If a key is not found in the config file, fall back to hardcoded English strings
4. **Hot Reload**: Support reloading config files without restarting the game (in debug mode)
5. **Validation**: Validate JSON structure on load and log errors
6. **Export Function**: Alt+Shift+E in debug mode exports all menu/dialog data for current language
7. **Example Files**: See `UserData/AokanaAccess/en/*.json.example` for reference format

## Creating Localization Files

1. **Export from game**:
   - Enable debug mode (F12)
   - Press Alt+Shift+E to export
   - Find exported data in `debug/{language}/menu_export.json`

2. **Create localization files**:
   - Copy example files from `UserData/AokanaAccess/en/*.json.example`
   - Rename to remove `.example` extension
   - Use exported IDs to add entries: `"MenuID.ButtonID": "Your Translation"`

3. **Test in game**:
   - Restart game or reload configs (if in debug mode)
   - Navigate to menus to verify translations appear correctly
