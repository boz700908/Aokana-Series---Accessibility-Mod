# Aokana Accessibility Mod v1.0.0

首个正式版本发布！为视障玩家提供完整的屏幕阅读器支持。

## 🎮 主要功能

### 屏幕阅读器支持
- 使用Tolk库支持NVDA、JAWS等主流屏幕阅读器
- 自动朗读菜单按钮、对话框、选择项
- 支持中文、英语、日语、繁体中文

### 菜单导航
- 标题菜单完整支持
- 游戏内菜单（保存、读取、设置等）
- 系统设置菜单
- 语音书签功能

### 翻译系统
- 硬编码翻译（86个按钮）
- 4种语言支持：简体中文、英语、日语、繁体中文
- 自动跟随游戏语言切换

### 调试工具
- F12：切换调试模式
- Alt+Shift+E：导出菜单ID和按钮图片（调试模式）

## 📦 安装方法

### 前置要求
- Aokana - Four Rhythms Across the Blue（合法购买）
- MelonLoader v0.7.1

### 安装步骤
1. 下载 `AokanaAccess-v1.0.0.zip`
2. 解压到游戏根目录
3. 确保文件结构：
   ```
   [游戏目录]/
   ├── Mods/
   │   └── AokanaAccess.dll
   └── UserData/
       └── AokanaAccess/
           ├── cn/messages.json
           ├── en/messages.json
           ├── jp/messages.json
           └── tc/messages.json
   ```
4. 启动游戏

## 🎯 使用说明

### 基本操作
- 使用方向键或Tab键在菜单中导航
- 屏幕阅读器会自动朗读当前焦点的按钮
- 按Enter或空格键激活按钮

### 调试功能
- 按F12启用调试模式（查看详细日志）
- 在调试模式下按Alt+Shift+E导出当前菜单的按钮ID

### 语言支持
模组会自动检测游戏语言并切换翻译：
- 简体中文 (cn)
- English (en)
- 日本語 (jp)
- 繁體中文 (tc)

## 🔧 技术细节

- **目标框架**: .NET Framework 3.5
- **Unity版本**: 2018.2.19f1
- **MelonLoader**: v0.7.1
- **架构**: x86 (32-bit)

## 📝 已知问题

- 部分动态生成的UI元素可能无法正确朗读
- 某些特殊场景下可能需要手动刷新焦点

## 🙏 致谢

感谢所有测试者的反馈和建议。

## 📄 许可证

本模组仅供辅助功能使用。Unity和游戏资源版权归原作者所有。

## 🔗 相关链接

- [GitHub仓库](https://github.com/boz700908/Aokana-Series---Accessibility-Mod)
- [构建指南](https://github.com/boz700908/Aokana-Series---Accessibility-Mod/blob/master/BUILD.md)
- [MelonLoader](https://github.com/LavaGang/MelonLoader)

---

**完整更新日志**: https://github.com/boz700908/Aokana-Series---Accessibility-Mod/blob/master/CHANGELOG.md
