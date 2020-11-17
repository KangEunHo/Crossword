using UnityEngine;
using System.Collections;

public static class VectorExtension
{
    public static Vector3 NewX(this Vector3 vector, float x)
    {
        return new Vector3(x, vector.y, vector.z);
    }

    public static Vector3 NewY(this Vector3 vector, float y)
    {
        return new Vector3(vector.x, y, vector.z);
    }

    public static Vector3 NewZ(this Vector3 vector, float z)
    {
        return new Vector3(vector.x, vector.y, z);
    }

    public static Vector2 NewX(this Vector2 vector, float x)
    {
        return new Vector2(x, vector.y);
    }

    public static Vector2 NewY(this Vector2 vector, float y)
    {
        return new Vector2(vector.x, y);
    }
}
