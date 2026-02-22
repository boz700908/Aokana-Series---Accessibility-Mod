# Building from Source

This guide explains how to build the Aokana Accessibility Mod from source code.

## Prerequisites

### Required Software
- **Visual Studio 2019 or later** (with .NET Framework 3.5 development tools)
  - OR **Visual Studio Code** with C# extension
  - OR **.NET SDK** with MSBuild
- **Git** (for cloning the repository)
- **Aokana - Four Rhythms Across the Blue** (legally owned copy)
- **MelonLoader v0.7.1** (installed to game directory)

### Required Game Files
You must have a legal copy of Aokana installed. The build process requires DLL files from the game.

## Setup Instructions

### 1. Clone the Repository

```bash
git clone https://github.com/boz700908/Aokana-Series---Accessibility-Mod.git
cd Aokana-Series---Accessibility-Mod
```

### 2. Install MelonLoader

Download and install MelonLoader v0.7.1 to your Aokana game directory:
https://github.com/LavaGang/MelonLoader/releases/tag/v0.7.1

### 3. Copy Required DLLs

Copy the following files from your game installation to the `libs/` directory:

**From MelonLoader** (`[Game]/MelonLoader/net35/`):
- `MelonLoader.dll`
- `0Harmony.dll`

**From Game** (`[Game]/Aokana_Data/Managed/`):
- `Assembly-CSharp.dll`
- `UnityEngine.dll`
- `UnityEngine.CoreModule.dll`
- `UnityEngine.UI.dll`
- `UnityEngine.UIModule.dll`
- `UnityEngine.TextRenderingModule.dll`
- `UnityEngine.IMGUIModule.dll`
- `Unity.TextMeshPro.dll`

Your `libs/` directory should look like this:
```
libs/
├── README.md
├── MelonLoader.dll
├── 0Harmony.dll
├── Assembly-CSharp.dll
├── UnityEngine.dll
├── UnityEngine.CoreModule.dll
├── UnityEngine.UI.dll
├── UnityEngine.UIModule.dll
├── UnityEngine.TextRenderingModule.dll
├── UnityEngine.IMGUIModule.dll
└── Unity.TextMeshPro.dll
```

### 4. Build the Project

#### Using Visual Studio
1. Open `AokanaAccess.csproj` in Visual Studio
2. Select **Release** configuration
3. Build → Build Solution (Ctrl+Shift+B)

#### Using Command Line
```bash
dotnet build AokanaAccess.csproj --configuration Release
```

#### Using MSBuild
```bash
msbuild AokanaAccess.csproj /p:Configuration=Release
```

### 5. Output Location

After successful build, the compiled DLL will be at:
```
bin/Release/AokanaAccess.dll
```

The build process automatically copies the DLL to your game's Mods folder (if configured in the project file).

## Installation

1. Copy `bin/Release/AokanaAccess.dll` to `[Game]/Mods/`
2. Copy `UserData/` folder to `[Game]/UserData/`
3. Launch the game

## Troubleshooting

### "Reference not found" errors
- Ensure all required DLLs are in the `libs/` directory
- Check that DLL versions match (Unity 2018.2.19f1, MelonLoader v0.7.1)

### Build succeeds but mod doesn't load
- Verify MelonLoader is properly installed
- Check `[Game]/MelonLoader/Latest.log` for errors
- Ensure target framework is .NET Framework 3.5

### "Could not load file or assembly" at runtime
- Make sure all Unity DLLs are from the same Unity version (2018.2.19f1)
- Verify game architecture matches (x86/32-bit)

## GitHub Actions

The repository includes a GitHub Actions workflow that automatically builds the mod on every push. However, due to licensing restrictions, Unity and game DLLs cannot be included in the public repository. The workflow downloads MelonLoader automatically but requires Unity DLLs to be provided separately.

## Development

For development instructions and code structure, see:
- `CLAUDE.md` - AI assistant development guide
- `docs/ACCESSIBILITY_MODDING_GUIDE.md` - Modding patterns and best practices
- `docs/technical-reference.md` - Technical reference

## License

This mod is provided as-is for accessibility purposes. Unity and game assets remain property of their respective owners.
