using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;

[InputControlLayout(stateType = typeof(LogitechDualActionInputReport))]
#if UNITY_EDITOR
[InitializeOnLoad] // Make sure static constructor is called during startup
#endif
public class LogitechDualActionHID : Gamepad
{
    static LogitechDualActionHID()
    {
        InputSystem.RegisterLayout<LogitechDualActionHID>("Logitech Dual Action (Unofficial)",
            new InputDeviceMatcher()
                .WithInterface("HID")
                .WithManufacturer("Logitech")
                .WithProduct("Logicech Dual Action"));

        InputSystem.RegisterLayout<LogitechDualActionHID>("Logitech Dual Action (Unofficial)",
            new InputDeviceMatcher()
                .WithInterface("HID")
                .WithManufacturer("Logicool")
                .WithProduct("Logicool Dual Action"));

        InputSystem.RegisterLayout<LogitechDualActionHID>("Logitech Dual Action (Unofficial)",
            new InputDeviceMatcher()
                .WithInterface("HID")
                .WithCapability("vendorId", 0x46D) // Logitech Inc.
                .WithCapability("productId", 0xC216)); // F310 Gamepad [DirectInput Mode]
    }

    [RuntimeInitializeOnLoadMethod]
    static void Init() { }
}
