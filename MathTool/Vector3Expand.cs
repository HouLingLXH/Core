using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Vector3Expand {

    //逆时针旋转
    public static Vector3 Vector3RotateInXZ(this Vector3 dir, float angle)
    {

        angle *= Mathf.Deg2Rad;
        float l_n_dirX = dir.x * Mathf.Cos(angle) - dir.z * Mathf.Sin(angle);
        float l_n_dirZ = dir.x * Mathf.Sin(angle) + dir.z * Mathf.Cos(angle);
        Vector3 l_dir = new Vector3(l_n_dirX, dir.y, l_n_dirZ);

        return l_dir;
    }

    //顺时针旋转
    public static Vector3 Vector3RotateInXZ2(this Vector3 dir, float angle)
    {

        angle *= Mathf.Deg2Rad;
        float l_n_dirX = dir.x * Mathf.Cos(angle) + dir.z * Mathf.Sin(angle);
        float l_n_dirZ = -dir.x * Mathf.Sin(angle) + dir.z * Mathf.Cos(angle);

        Vector3 l_dir = new Vector3(l_n_dirX, dir.y, l_n_dirZ);

        return l_dir;
    }
}
