using UnityEngine;

public class MobileInput : MonoBehaviour
{
    public static bool leftPressed;
    public static bool rightPressed;
    public static bool jumpPressed;

    public void LeftDown() => leftPressed = true;
    public void LeftUp() => leftPressed = false;

    public void RightDown() => rightPressed = true;
    public void RightUp() => rightPressed = false;

    public void JumpDown() => jumpPressed = true;
    public void JumpUp() => jumpPressed = false;
}
