# Unity Runtime Input Debug
Simpe unity input debul tool that displays currently pressed input on the screen.

Useful when recording gameplay demo for issue reporting or demonstrating how input is handled and translated into the game.

**Current implementation depends on the legacy Unity Input System. I plan to extend the tools to support the new input system.**

## Table of Contents
[Features](#features)<br />
[Installation](#installation)<br />
[Usage](#usage)<br />
[Examples](#examples)<br />
[Credits](#examples)<br />

## Features
- Input capture

Displays currently held/active input on the screen

![InputCapture](https://github.com/ntstojchev/UnityRuntimeInputDebug/blob/main/Docs/InputCaptureExample.gif)

- Input history

Displays latest handled input in a queue

![InputHistory](https://github.com/ntstojchev/UnityRuntimeInputDebug/blob/main/Docs/InputHistoryCaptureExample.gif)

## Installation
- Copy [Assets/RuntimeInputDebugger](https://github.com/ntstojchev/UnityRuntimeInputDebug/tree/main/Assets/RuntimeInputDebugger) folder to your project
- Download [latest release](https://github.com/ntstojchev/UnityRuntimeInputDebug/releases) zip and unzip it's content to your project
- [Download the URID_Package](https://github.com/ntstojchev/UnityRuntimeInputDebug/raw/main/URID_Package.unitypackage) and import it in your project

## Usage

Main entry point for the debugger is the [**InputDebugger component**](https://github.com/ntstojchev/UnityRuntimeInputDebug/blob/main/Assets/RuntimeInputDebugger/Code/InputDebugger/InputDebugger.cs).
The component and the code is documented with comments, tags and tooltips.

### Default integration into projects
The debugger includes default prefab (oreset) that can be used or extended in most of the cases.
- Instantiate **InputDebugger** prefab when you need the input debugger or put it in the scene you want the debugger to be active
- **InputDebugger** can be found in the Prefabs folder
- The default preset:
  - has the input debugger enabled by default
  - supports most of the keyboard keys and gamepad buttons
  - handles small number of the default axis from the Input Project Settings - Horizontal, Vertical, Mouse X, Mouse Y, Mouse ScrollWheel
  - is anchored in the top left part of the screen
  - doesn't capture mouse position and doesn't display input history queue

![DefaultDebuggerSettings](https://github.com/ntstojchev/UnityRuntimeInputDebug/blob/main/Docs/DefaultInputDebuggerComponentSettings.png)

### Custom integration


## Examples

Example usage of the tool in the Unity Open Project - Chop Chop!:

![CaptureExample](https://github.com/ntstojchev/UnityRuntimeInputDebug/blob/main/Docs/UOP_InputCaptureExample_Light.gif)

## Credits

- [Input prompts assets by Kenney Assets](https://www.kenney.nl/assets/input-prompts-pixel-16)
- [Unity Open Project - Chop Chop](https://github.com/UnityTechnologies/open-project-1)
