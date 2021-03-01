using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalSettings : MonoBehaviour
{
    public static bool usingMouse = true;

    public static KeyCode keyUse = KeyCode.E;
    public static KeyCode keyUse2 = KeyCode.F;
    public static KeyCode keyEscape = KeyCode.Escape;
    public static KeyCode keyEscapeDebug = KeyCode.Q;
    public static string PrimaryAxisName = "Primary";

    public static bool PressingConfirm()
    {
        return Input.GetAxis(PrimaryAxisName) > 0 || Input.GetKey(KeyCode.Return);
    }

    public static bool PressingEscape()
    {
        return Input.GetKey(keyEscape) || Input.GetKey(keyEscapeDebug);
    }

    public static bool PressingUse()
    {
        return Input.GetKey(keyUse2) || Input.GetKey(keyUse);
    }
}
