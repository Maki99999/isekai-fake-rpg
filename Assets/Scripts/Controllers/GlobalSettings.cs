using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalSettings : MonoBehaviour
{
    public static bool usingMouse = true;

    public static KeyCode keyUse = KeyCode.E;
    public static KeyCode keyUse2 = KeyCode.F;
    public static KeyCode keyEscape = KeyCode.Escape;
    public static KeyCode keyEscape2 = KeyCode.Q;

    public static bool Confirm()
    {
        return Input.GetAxis("Primary Fire") > 0 || Input.GetKey(KeyCode.Return);
    }
}
