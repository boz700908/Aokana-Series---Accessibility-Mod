# Changelog

All notable changes to the Aokana Accessibility Mod will be documented in this file.

## [Unreleased]

### Fixed
- 修复选项菜单焦点冲突 - 当ChoiceHandler激活时MenuHandler不再干扰，避免重复朗读
- 修复编译配置 - 禁用TargetFrameworkAttribute生成以兼容.NET Framework 3.5
- 移除角色名按钮的硬编码翻译，改用游戏原生文本

### Changed
- 优化选项菜单导航体验，确保只朗读选项文本而不是角色名

## [1.0.0] - 2026-02-20

### Added
- Initial release with full accessibility support
- Dialog text announcement with multi-language support
- Character name announcement
- Auto-read mode (enabled by default)
- Choice navigation and announcement
- Backlog accessibility with full keyboard navigation
- Main menu navigation with announcements
- Settings menu accessibility (Alt+O shortcut)
- Voice bookmarks menu (F11 shortcut)
- Confirmation dialog support
- Quick save/load features (F8/F9)
- Quick voice bookmark (F10)
- Slot save/load shortcuts (Shift+1-9, Alt+1-9)
- Multi-language support (English, Simplified Chinese, Traditional Chinese, Japanese)
- Debug mode toggle (F12)
- Support for both Aokana main game and EXTRA1

### Technical
- Built on MelonLoader v0.7.1
- Uses Harmony for runtime patching
- Tolk library for screen reader integration
- .NET Framework 3.5 target
- 32-bit (x86) architecture support
