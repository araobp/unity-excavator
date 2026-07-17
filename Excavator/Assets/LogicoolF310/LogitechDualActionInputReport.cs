using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.InputSystem.Layouts;

[StructLayout(LayoutKind.Explicit, Size = 8)]
struct LogitechDualActionInputReport : IInputStateTypeInfo
{
    public FourCC format => new FourCC('H', 'I', 'D');

    [InputControl(name = "leftStick", format = "VC2B", layout = "Stick", displayName = "Left Stick", offset = 0)]
    [InputControl(name = "leftStick/x", defaultState = 127, format = "BYTE", parameters = "normalize,normalizeMin=0,normalizeMax=1,normalizeZero=0.5")]
    [InputControl(name = "leftStick/left", parameters = "normalize,normalizeMin=0,normalizeMax=1,normalizeZero=0.5,clamp=2,clampMin=-1,clampMax=0,invert")]
    [InputControl(name = "leftStick/right", parameters = "normalize,normalizeMin=0,normalizeMax=1,normalizeZero=0.5,clamp=2,clampMin=0,clampMax=1")]
    [InputControl(name = "leftStick/y", offset = 1, defaultState = 127, format = "BYTE", parameters = "normalize,normalizeMin=0,normalizeMax=1,normalizeZero=0.5,invert")]
    [InputControl(name = "leftStick/down", offset = 1, parameters = "normalize,normalizeMin=0,normalizeMax=1,normalizeZero=0.5,clamp=2,clampMin=0,clampMax=1")]
    [InputControl(name = "leftStick/up", offset = 1, parameters = "normalize,normalizeMin=0,normalizeMax=1,normalizeZero=0.5,clamp=2,clampMin=-1,clampMax=0,invert")]
    [FieldOffset(0)] public byte leftStickX;
    [FieldOffset(1)] public byte leftStickY;

    [InputControl(name = "rightStick", format = "VC2B", layout = "Stick", displayName = "Right Stick", offset = 2)]
    [InputControl(name = "rightStick/x", defaultState = 127, format = "BYTE", parameters = "normalize,normalizeMin=0,normalizeMax=1,normalizeZero=0.5")]
    [InputControl(name = "rightStick/left", parameters = "normalize,normalizeMin=0,normalizeMax=1,normalizeZero=0.5,clamp=2,clampMin=-1,clampMax=0,invert")]
    [InputControl(name = "rightStick/right", parameters = "normalize,normalizeMin=0,normalizeMax=1,normalizeZero=0.5,clamp=2,clampMin=0,clampMax=1")]
    [InputControl(name = "rightStick/y", offset = 1, defaultState = 127, format = "BYTE", parameters = "normalize,normalizeMin=0,normalizeMax=1,normalizeZero=0.5,invert")]
    [InputControl(name = "rightStick/up", offset = 1, parameters = "normalize,normalizeMin=0,normalizeMax=1,normalizeZero=0.5,clamp=2,clampMin=0,clampMax=1")]
    [InputControl(name = "rightStick/down", offset = 1, parameters = "normalize,normalizeMin=0,normalizeMax=1,normalizeZero=0.5,clamp=2,clampMin=-1,clampMax=0,invert")]
    [FieldOffset(2)] public byte rightStickX;
    [FieldOffset(3)] public byte rightStickY;

    [InputControl(name = "dpad", displayName = "D-Pad", format = "BIT", layout = "Dpad", sizeInBits = 4, defaultState = 8)]
    [InputControl(name = "dpad/up", displayName = "D-Pad Down", format = "BIT", layout = "DiscreteButton", parameters = "minValue=7,maxValue=1,nullValue=8,wrapAtValue=7", bit = 0, sizeInBits = 4)]
    [InputControl(name = "dpad/right", displayName = "D-Pad Left", format = "BIT", layout = "DiscreteButton", parameters = "minValue=1,maxValue=3", bit = 0, sizeInBits = 4)]
    [InputControl(name = "dpad/down", displayName = "D-Pad Right", format = "BIT", layout = "DiscreteButton", parameters = "minValue=3,maxValue=5", bit = 0, sizeInBits = 4)]
    [InputControl(name = "dpad/left", displayName = "D-Pad Up", format = "BIT", layout = "DiscreteButton", parameters = "minValue=5, maxValue=7", bit = 0, sizeInBits = 4)]
    [InputControl(name = "buttonWest", displayName = "X", layout = "Button", format = "BIT", bit = 4)]
    [InputControl(name = "buttonSouth", displayName = "A", layout = "Button", format = "BIT", bit = 5)]
    [InputControl(name = "buttonEast", displayName = "B", layout = "Button", format = "BIT", bit = 6)]
    [InputControl(name = "buttonNorth", displayName = "Y", layout = "Button", format = "BIT", bit = 7)]
    [FieldOffset(4)] public byte buttons1;

    [InputControl(name = "leftShoulder", displayName = "Left Bumper", layout = "Button", format = "BIT", offset = 5, bit = 0)]
    [InputControl(name = "rightShoulder", displayName = "Right Bumper", layout = "Button", format = "BIT", offset = 5, bit = 1)]
    [InputControl(name = "leftTrigger", displayName = "Left Trigger", layout = "Button", format = "BIT", offset = 5, bit = 2)]
    [InputControl(name = "rightTrigger", displayName = "Right Trigger", layout = "Button", format = "BIT", offset = 5, bit = 3)]
    [InputControl(name = "back", displayName = "Back", layout = "Button", format = "BIT", offset = 5, bit = 4)]
    [InputControl(name = "start", displayName = "Start", layout = "Button", format = "BIT", offset = 5, bit = 5)]
    [InputControl(name = "leftStickPress", displayName = "Left Stick Press", layout = "Button", format = "BIT", offset = 5, bit = 6)]
    [InputControl(name = "rightStickPress", displayName = "Right Stick Press", layout = "Button", format = "BIT", offset = 5, bit = 7)]
    [FieldOffset(5)] public byte buttons2;

    [InputControl(name = "mode", displayName = "Mode", layout = "Button", format = "BIT", offset = 6, bit = 3)]
    [FieldOffset(6)] public byte buttons3;

    [InputControl(name = "reserve1", layout = "Button", format = "BYTE", offset = 7, bit = 0)]
    [FieldOffset(7)] public byte reserve1;
}
