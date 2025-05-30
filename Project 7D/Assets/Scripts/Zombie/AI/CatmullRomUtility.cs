using System.Collections.Generic;
using UnityEngine;

public static class CatmullRomUtility
{
    public static List<Vector3> GetSmoothPath(List<Vector3> points, int interpolationCount = 10)
    {
        List<Vector3> result = new();

        for (int i = 0; i < points.Count - 1; i++)
        {
            Vector3 p0 = i == 0 ? points[i] : points[i - 1];
            Vector3 p1 = points[i];
            Vector3 p2 = points[i + 1];
            Vector3 p3 = i + 2 < points.Count ? points[i + 2] : p2;

            for (int j = 0; j < interpolationCount; j++)
            {
                float t = j / (float)interpolationCount;
                result.Add(CatmullRom(p0, p1, p2, p3, t));
            }
        }

        result.Add(points[^1]); // 마지막 점 추가
        return result;
    }

    static Vector3 CatmullRom(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        return 0.5f * (
            2f * p1 +
            (-p0 + p2) * t +
            (2f * p0 - 5f * p1 + 4f * p2 - p3) * t * t +
            (-p0 + 3f * p1 - 3f * p2 + p3) * t * t * t
        );
    }
}
