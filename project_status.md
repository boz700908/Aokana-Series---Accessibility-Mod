# Aokana Series - Accessibility Mod

## Project Information

- **Games Supported:**
  - Aokana - Four Rhythms Across the Blue
  - Aokana - Four Rhythms Across the Blue - EXTRA1
- **Developer:** NekoNyanSoft
- **Engine:** Unity 2018.2.19f1
- **Architecture:** 32-bit (x86)
- **Mod Loader:** MelonLoader v0.7.1
- **Runtime:** net35 (Mono)
- **Game Directories:**
  - Main: D:\SteamLibrary\steamapps\common\Aokana
  - EXTRA1: D:\SteamLibrary\steamapps\common\Aokana Extra1
- **User Experience Level:** Beginner
- **User Game Familiarity:** Somewhat

## Multi-Game Support

This mod supports both Aokana games with a single DLL:
- Automatically detects which game is running
- Uses the same codebase for both games (they share the same engine and UI structure)
- Build system automatically copies the mod to both game directories

**Note:** EXTRA2 is not supported due to MelonLoader compatibility issues with that specific game.

## Language Support

The mod provides full localization in four languages:
- **English** (en) - Default
- **简体中文** (Simplified Chinese - cn)
- **繁體中文** (Traditional Chinese - tc)
- **日本語** (Japanese - jp)

The mod automatically detects the game's language setting and switches announcements accordingly. All menu navigation, status messages, and notifications are localized.

## Current Phase

**Phase 3: Feature Development** - Complete

All accessible features have been implemented for both supported games. Final code cleanup and documentation completed on 2026-02-20.

### Completed Features

#### Core Dialog & Gameplay (Priority 1 & 2)
- ✅ Dialog text announcement with multi-language support (WORKING)
- ✅ Character name announcement (WORKING)
- ✅ Auto-read mode (enabled by default) (WORKING)
- ✅ Choice announcement (WORKING)
- ✅ Choice navigation with Up/Down arrows (WORKING)

#### Convenience Features (Priority 3)
- ✅ F1 - Repeat last dialog (WORKING)
- ✅ F2 - Repeat character name (WORKING)
- ✅ F3 - Repeat all choices (WORKING)
- ✅ F4 - Repeat current choice (WORKING)
- ✅ F6 - Toggle auto-read mode (WORKING)
- ✅ F12 - Toggle debug mode (WORKING)

#### Backlog Navigation
- ✅ Backlog accessibility (WORKING)
  - Announces when backlog opens/closes
  - Announces topmost entry when scrolling with Up/Down/PageUp/PageDown
  - Announces beginning/end of backlog
  - F5 - Read all visible entries (4 at a time)

#### Menu Accessibility (Priority 4)
- ✅ Main menu navigation (WORKING)
  - Announces "Main menu" when opened
  - Announces button text when navigating with arrow keys
  - F7 - Repeat current menu item
  - Supports main menu and extra menu buttons
  - Announces when extra menu closes

- ✅ Confirmation dialogs (WORKING)
  - Announces dialog question text when opened
  - Dialog text not interrupted by auto-focus
  - Initial button focus is silent (no announcement)
  - Announces Yes/No buttons only when navigating with arrow keys
  - No duplicate announcements when closing dialog
  - Parent menu continues working after dialog closes

- ✅ Settings menu (WORKING)
  - Alt+O - Open settings menu shortcut
  - Fixed slider values being announced at game startup
  - Fixed tab switching announcements
  - Added debouncing to prevent duplicate announcements
  - Implemented auto-focus with suppression for programmatic focus changes
  - Slider values announce when navigating with keyboard
  - All 4 tabs (Visual, Text, Sound, Voice) working correctly
  - Announces when menu closes (unless dialog pops up)

- ✅ Voice bookmarks menu (WORKING)
  - F11 - Open voice bookmarks menu
  - Announces "Voice bookmarks" when opened
  - Auto-focuses first item for immediate keyboard navigation
  - Announces slot number and bookmark text when navigating
  - Announces "Empty" for empty slots
  - F7 - Repeat current item
  - Fixed menu title being interrupted by auto-focus announcement
  - Announces when menu closes

- ❌ Save/Load menu (REMOVED)
  - Removed all save/load menu support due to technical limitations
  - Game uses EventTrigger instead of standard Unity UI buttons
  - Keyboard navigation not feasible without major game modifications

- ✅ Menu close announcements (WORKING)
  - All menus announce when closed
  - Suppressed when confirmation dialog pops up
  - Extra menu announces when closed

#### Quick Access Features
- ✅ Quick save/load and voice bookmarks (WORKING)
  - F8 - Quick save with confirmation announcement
  - F9 - Quick load (shows confirmation dialog)
  - F10 - Quick voice bookmark with confirmation

### Pending Tasks

None - All accessible features have been implemented.

### Excluded Features

- Gallery menu - Contains only image galleries (CG, sprites), not accessible for screen reader users

## Setup Status

- [x] Game path identified
- [x] MelonLoader detected (already installed)
- [x] Tolk DLLs verified (32-bit)
- [x] .NET SDK verified (10.0.103)
- [x] dnSpy installed
- [x] Game code decompiled to `decompiled/`
- [x] C# project created
- [x] Basic framework implemented (ScreenReader, Loc, DebugLogger, Main)
- [x] First test build successful - mod loads and announces correctly

## Game Type

Visual novel with choice-based gameplay. Key systems identified from decompiled code:
- `AdvChoice.cs` - Dialog choices
- `Backlog.cs` - Text history
- `EngineMain.cs` - Main game engine
- `UIConf.cs` - Settings menu
- `UIVBookmark.cs` - Voice bookmarks menu

## Keybindings

- **F1** - Repeat last dialog text
- **F2** - Repeat character name
- **F3** - Repeat all choices (when choices active)
- **F4** - Repeat current choice (when choices active)
- **F5** - Read all visible backlog entries (when backlog active)
- **F6** - Toggle auto-read mode
- **F7** - Repeat current menu item (when in menus)
- **F8** - Quick save
- **F9** - Quick load
- **F10** - Quick voice bookmark
- **F11** - Open voice bookmarks menu
- **F12** - Toggle debug mode
- **Alt+O** - Open settings menu
- **Up/Down arrows** - Navigate choices / backlog / menus (game native)
- **PageUp/PageDown** - Navigate backlog (game native)

## Notes

- User prefers communication in Chinese where appropriate
- Game is a visual novel, so text-to-speech for dialog and menu navigation are primary features
- All core gameplay features are now fully accessible
- Settings and voice bookmarks menus are fully functional
- Save/Load menu support was removed due to technical limitations with the game's UI implementation

## Recent Changes (2026-02-20)

### Code Cleanup
- Removed subtitle support feature (not needed)
- Removed mod loaded announcement at startup
- Updated all documentation to reflect current features
- Cleaned up Main.cs OnUpdate() method
