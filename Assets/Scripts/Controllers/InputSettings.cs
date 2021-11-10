using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputSettings
{
    public static bool usingMouse = true;

    static KeyCode[] keysUse = {KeyCode.E};
    static KeyCode[] keysEscape = {KeyCode.Escape, KeyCode.X};
    static KeyCode[] keysLook = {KeyCode.Q};
    static KeyCode[] keysStand = {KeyCode.F};
    static KeyCode[] keysLevel = {KeyCode.L};
    static string PrimaryAxisName = "Primary";

    private static bool PressingButton(KeyCode[] buttons)
    {
        foreach(KeyCode key in buttons)
            if(Input.GetKey(key))
                return true;
        return false;
    }

    public static bool PressingConfirm()
    {
        return Input.GetAxis(PrimaryAxisName) > 0 || Input.GetKey(KeyCode.Return) || InputSettings.PressingUse();
    }

    public static bool PressingUse()
    {
        return PressingButton(keysUse);
    }

    public static bool PressingEscape()
    {
        return PressingButton(keysEscape);
    }

    public static bool PressingLook()
    {
        return PressingButton(keysLook);
    }

    public static bool PressingStand()
    {
        return PressingButton(keysStand);
    }

    public static bool PressingLevel()
    {
        return PressingButton(keysLevel);
    }
}
