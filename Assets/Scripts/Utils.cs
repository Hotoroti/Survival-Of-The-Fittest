using UnityEngine;

public static class Utils
{
    public static bool IsOnGround(Vector3 target)
    {
        return Physics.Raycast(target, Vector3.down, 1f, LayerMask.GetMask("Ground"));
    }
}
