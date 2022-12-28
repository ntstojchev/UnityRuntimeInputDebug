# Unity Runtime Input Debug
Simple unity input debug tool that displays currently pressed input on the screen.

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
The main component that you need to use is the `InputDebugger` component. The debugger is based on IMGUI so it's supposed to work wherever you attach it or instantiate it.
- To use the debugger:
  - You can toggle the debugger on/off from the `Enabled` value either from code or from the inspector.
  - You must assign `InputDebugCaptureActions` scriptable object in the `Input Actions` field
  - `InputDebugCaptureActions` is a collection of all the keys and axis that the debugger must listen for. The collection is composed from Keys and Axis collections.
    - `Keys` collection contains `KeyInputAction`. If `Icon` is not assigned, the `Name` will be used during debugging.
![KeyInputActionExample](https://github.com/ntstojchev/UnityRuntimeInputDebug/blob/main/Docs/KeyInputActionExample.png)
    - `Axes` collection contains `AxisInputAction`. If  `PositiveValueIcon` and/or `NegativeValueIcon` are not assigned, `AxisName` will be used and the value will be displayed based on the `AxisValueLabelPattern` set in the `InputDebugger`.
![AxisInputActionExampe](https://github.com/ntstojchev/UnityRuntimeInputDebug/blob/main/Docs/AxisInputActionExample.png)

  - `DebugTextStyle` is `GUIStyle` so you can configure it by your preferences. _Note that the default value for the `Normal` style is BLACK!_
  - `Draw Anchor` can be used to alter the position of the debugger. Currently supported - `TopLeft, TopCenter, TopRight, MiddleLeft, Middle, MiddleRight, BottomLeft, BottomCenter, BottomRight`.

- To use the input history queue:
  - Enable it by toggling the `InputHistoryEnabled` property either by code or by the inspector

## Examples

Example usage of the debugger in the Unity Open Project - Chop Chop!:

![CaptureExample](https://github.com/ntstojchev/UnityRuntimeInputDebug/blob/main/Docs/UOP_InputCaptureExample_Light.gif)

## Credits

- [Input prompts assets by Kenney Assets](https://www.kenney.nl/assets/input-prompts-pixel-16)
- [Unity Open Project - Chop Chop](https://github.com/UnityTechnologies/open-project-1)
