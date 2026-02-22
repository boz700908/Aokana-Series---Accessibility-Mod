# Required Libraries

This directory should contain the following DLL files for compilation:

## MelonLoader Dependencies (v0.7.1)
- `MelonLoader.dll` - MelonLoader core library
- `0Harmony.dll` - Harmony patching library

Download from: https://github.com/LavaGang/MelonLoader/releases/tag/v0.7.1

## Unity Engine Dependencies (Unity 2018.2.19f1)
These files should be copied from your Aokana game installation:

From `[Game]/Aokana_Data/Managed/`:
- `UnityEngine.dll`
- `UnityEngine.CoreModule.dll`
- `UnityEngine.UI.dll`
- `UnityEngine.UIModule.dll`
- `UnityEngine.TextRenderingModule.dll`
- `UnityEngine.IMGUIModule.dll`

## Game Assembly
From `[Game]/Aokana_Data/Managed/`:
- `Assembly-CSharp.dll` - Game code assembly

## TextMeshPro
From `[Game]/Aokana_Data/Managed/`:
- `Unity.TextMeshPro.dll`

## Installation for Local Development

1. Install MelonLoader v0.7.1 to your Aokana game directory
2. Copy the required DLLs from the game's Managed folder to this `libs/` directory
3. The project should now compile successfully

## GitHub Actions

The GitHub Actions workflow will automatically download MelonLoader dependencies.
Unity and game DLLs should be committed to this directory or provided through another method.

**Note:** Due to licensing restrictions, Unity DLLs and game assemblies cannot be distributed publicly.
Developers must obtain these files from their own legal copy of the game.
