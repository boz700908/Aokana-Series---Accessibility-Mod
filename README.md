# Aokana Series - Accessibility Mod

A comprehensive accessibility mod for the Aokana visual novel series, providing full screen reader support for blind and visually impaired players.

## Supported Games

This mod works with two Aokana games:
- **Aokana - Four Rhythms Across the Blue** (Main game)
- **Aokana - Four Rhythms Across the Blue - EXTRA1**

**Note:** EXTRA2 is not supported due to MelonLoader compatibility issues with that specific game.

## Language Support

The mod automatically detects the game's language setting and provides all announcements in:
- **English** (en)
- **简体中文** (Simplified Chinese - cn)
- **繁體中文** (Traditional Chinese - tc)
- **日本語** (Japanese - jp)

All mod announcements (menu navigation, button labels, status messages, confirmation dialogs, etc.) will match your game's language setting. The mod uses the same language as your game automatically.

## Features

### Core Gameplay
- **Dialog Text Announcement**: Automatically reads all dialog text with multi-language support
- **Character Name Announcement**: Announces character names when they speak
- **Auto-Read Mode**: Enabled by default, automatically reads new dialog
- **Choice Navigation**: Navigate choices with Up/Down arrow keys
- **Choice Announcement**: Announces all available choices

### Backlog
- **Backlog Accessibility**: Full keyboard navigation of text history
- **Entry Announcement**: Announces entries when scrolling
- **Batch Reading**: F5 reads all visible entries (4 at a time)
- **Beginning/End Notifications**: Announces when reaching backlog limits

### Menu Navigation
- **Main Menu**: Full keyboard navigation with announcements
- **Settings Menu**: Alt+O shortcut, all tabs accessible
- **Voice Bookmarks**: F11 to open, full keyboard navigation
- **Confirmation Dialogs**: Announces dialog text and button navigation
- **Menu Close Announcements**: All menus announce when closed

### Quick Access
- **F1**: Repeat last dialog
- **F2**: Repeat character name
- **F3**: Repeat all choices
- **F4**: Repeat current choice
- **F5**: Read all visible backlog entries
- **F6**: Toggle auto-read mode
- **F7**: Repeat current menu item
- **F8**: Quick save
- **F9**: Quick load
- **F10**: Quick voice bookmark
- **F11**: Open voice bookmarks menu
- **F12**: Toggle debug mode
- **Alt+O**: Open settings menu

### Slot Save/Load
- **Shift+1-9**: Save to slot 1-9 (bypasses menu)
- **Alt+1-9**: Load from slot 1-9 (bypasses menu)

## Installation

1. Install MelonLoader v0.7.1 for each game you want to play
2. Copy `AokanaAccess.dll` to the `Mods` folder in each game directory:
   - Main game: `[Game Directory]\Mods\`
   - EXTRA1: `[Game Directory]\Mods\`
3. Launch the game - the mod will automatically detect which game is running

## Building from Source

Requirements:
- .NET SDK (any version that supports .NET Framework 3.5)
- Visual Studio or VS Code (optional)

Build command:
```bash
dotnet build AokanaAccess.csproj
```

The build system automatically copies the compiled DLL to both game directories.

## Technical Details

- **Engine**: Unity 2018.2.19f1
- **Architecture**: 32-bit (x86)
- **Mod Loader**: MelonLoader v0.7.1
- **Runtime**: .NET Framework 3.5 (Mono)
- **Screen Reader**: Tolk library (supports NVDA, JAWS, SAPI, etc.)

## Excluded Features

- **Gallery Menu**: Contains only visual content (CG images, sprites), not accessible for screen reader users

## Credits

Developed using:
- MelonLoader - Mod loader framework
- Harmony - Runtime patching library
- Tolk - Screen reader integration library

## License

This is a fan-made accessibility mod. All game content and trademarks belong to NekoNyanSoft.
