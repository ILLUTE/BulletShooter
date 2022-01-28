using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Methods
{
    public static bool Remove(this Collider[] x, int length, Collider collider)
    {
        int index = -1;

        bool removed = false;

        for (int i = 0; i < length; i++)
        {
            if (x[i] == collider)
            {
                index = i;
                break;
            }
        }

        if (index != -1)
        {
            removed = true;
            for (int i = index; i < length - 1; i++)
            {
                x[i] = x[i + 1];
            }
        }

        return removed;
    }

    public static void DrawWireArc(Vector3 position, Vector3 dir, float anglesRange, float radius, float maxSteps = 20)
    {
        var srcAngles = GetAnglesFromDir(position, dir);
        var initialPos = position;
        var posA = initialPos;
        var stepAngles = anglesRange / maxSteps;
        var angle = srcAngles - anglesRange / 2;
        for (var i = 0; i <= maxSteps; i++)
        {
            var rad = Mathf.Deg2Rad * angle;
            var posB = initialPos;
            posB += new Vector3(radius * Mathf.Cos(rad), 0, radius * Mathf.Sin(rad));

            Gizmos.DrawLine(posA, posB);

            angle += stepAngles;
            posA = posB;
        }
        Gizmos.DrawLine(posA, initialPos);
    }

    static float GetAnglesFromDir(Vector3 position, Vector3 dir)
    {
        var forwardLimitPos = position + dir;
        var srcAngles = Mathf.Rad2Deg * Mathf.Atan2(forwardLimitPos.z - position.z, forwardLimitPos.x - position.x);

        return srcAngles;
    }

    public static Color SetAlpha(this Color a, float alpha)
    {
        a = new Color(a.r, a.g, a.b, alpha);

        return a;
    }
}
