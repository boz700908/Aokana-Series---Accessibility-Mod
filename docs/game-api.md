# Aokana - Game API Documentation

This document contains information about the game's internal structure discovered through code analysis.

## Core Classes

### EngineMain
- **Location:** `EngineMain.cs`
- **Access:** `EngineMain.engine` (static singleton)
- **Purpose:** Main game engine controller

**Key Properties:**
- `advmain` (UIAdv) - The adventure/visual novel UI controller
- `skipMode` (int) - Current skip mode state
- `uiflow` (UIFlow) - UI flow controller

### UIAdv
- **Location:** `UIAdv.cs`
- **Access:** `EngineMain.engine.advmain`
- **Purpose:** Handles visual novel dialog display

**Key Properties:**
- `advtext` (TextMeshProUGUI) - The dialog text component
- `nametext` (TextMeshProUGUI) - The character name text component
- `choice` (AdvChoice) - The choice menu handler
- `histtmp` (string[]) - Temporary history storage [0]=name, [1]=text

**Key Methods:**
- `ShowText(string txin, string dispnamein, bool updateonly)` - Called when new dialog is displayed
  - `txin` - The dialog text
  - `dispnamein` - The character name
  - `updateonly` - If true, updates existing text without animation

### AdvChoice
- **Location:** `AdvChoice.cs`
- **Access:** `EngineMain.engine.advmain.choice`
- **Purpose:** Handles choice menus

**Key Properties:**
- `active` (bool) - Whether choice menu is currently displayed
- `items` (ChoiceItem[]) - Array of 3 choice items
  - Each item has: `obj` (GameObject), `tx` (TextMeshProUGUI with choice text)

**Key Methods:**
- `ShowMenu(List<string>[] mplist, string id)` - Displays a choice menu
  - `mplist` - Array of string lists (one per language)
  - `id` - Menu identifier
- `HideMenu()` - Hides the choice menu
- `ChoiceClick(GameObject obj)` - Called when a choice is selected

## Text Display System

**Flow:**
1. Game calls `UIAdv.ShowText(text, name, updateonly)`
2. Text is split by language using `UtilText.SplitLangStr()`
3. Character name is set in `nametext.text`
4. Dialog text is set in `advtext.text` (with fade animation if not skipping)
5. Text is stored in `histtmp[0]` (name) and `histtmp[1]` (text) for backlog

**Important Notes:**
- Text uses TextMeshPro (TMPro) components, not standard Unity Text
- Text may contain formatting tags that need to be preserved or stripped
- The game supports multiple languages via `EngineMain.lang`

## Choice System

**Flow:**
1. Game calls `AdvChoice.ShowMenu(choices, id)`
2. Up to 3 choices are displayed
3. Player can navigate with Up/Down arrow keys
4. Player selects with Enter or mouse click
5. `AdvChoice.ChoiceClick()` is called
6. Choice menu is hidden and game continues

**Important Notes:**
- Choices are stored in `items[i].tx.text`
- The `active` property indicates if choices are currently shown
- Arrow keys are already used by the game for choice navigation

## Game Key Bindings

**Used by Game:**
- PageUp / UpArrow / ScrollWheel Up - Open backlog
- DownArrow - Navigate choices (when active)
- RightClick - Stop skipping

**Safe for Mod:**
- F1-F11 (F12 already used for debug toggle)
- NumPad keys
- Ctrl/Alt/Shift combinations
- Letters that don't conflict with game shortcuts

## Backlog System

### UIBacklog
- **Location:** `UIBacklog.cs`
- **Access:** Via `EngineMain.engine.uiflow`
- **Purpose:** Displays text history

**Key Properties:**
- `b` (Backlog) - The backlog data (private field)
- `idxBegin` (int) - Starting index of visible entries (private field)
- `msgframe` (MsgFrame[4]) - Array of 4 visible entry frames

**Key Methods:**
- `ShowBacklog()` - Opens the backlog screen
- `HideBacklog()` - Closes the backlog screen
- `RefreshUI()` - Updates the display when scrolling
- `Scroll(int dist, bool exit)` - Scrolls by dist entries

**Navigation:**
- UpArrow - Scroll up one entry
- DownArrow - Scroll down one entry
- PageUp - Scroll up 4 entries
- PageDown - Scroll down 4 entries
- Mouse wheel - Scroll

### Backlog
- **Location:** `Backlog.cs`
- **Purpose:** Stores history entries

**Key Properties:**
- `entries` (List<BacklogEntry>) - List of all backlog entries

### BacklogEntry
- **Location:** `BacklogEntry.cs`
- **Purpose:** Single history entry

**Key Properties:**
- `scr` (ScriptEntry) - Contains `charname` and `text` (multi-language format)

**Text Format:**
- Same as dialog: uses '␂' (U+2402) separator for multiple languages
- Extract using same logic as `UIAdvPatches.ExtractCurrentLanguageText()`

## Next Steps for Implementation

1. ✅ Hook `UIAdv.ShowText()` to announce dialog text and character names
2. ✅ Hook `AdvChoice.ShowMenu()` to announce available choices
3. ✅ Add navigation keys for reading choices
4. Hook `UIBacklog.ShowBacklog()` to announce backlog opened
5. Hook `UIBacklog.RefreshUI()` to announce current entry when scrolling
6. Add menu navigation support
