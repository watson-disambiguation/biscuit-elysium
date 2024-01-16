using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Line
{
    const float verticalGradient = 1e5f;
    float gradient;
    float yIntercept;
    Vector2 pointOnLine_1;
    Vector2 pointOnLine_2;

    float Perpendicular;

    bool approachSide;
    public Line(Vector2 pointOnLine, Vector2 pointPerpendicular)
    {
        gradient = 1f;
        yIntercept = 0f;
        float dx = pointOnLine.x - pointPerpendicular.x;
        float dy = pointOnLine.y - pointPerpendicular.y;

        if (dx == 0) { Perpendicular = verticalGradient; }
        else { Perpendicular = dy/dx; }

        if (Perpendicular == 0) { Perpendicular = verticalGradient; }
        else { gradient = -1f / Perpendicular; }

        yIntercept = pointOnLine.y - pointOnLine.x * gradient;
        pointOnLine_1 = pointOnLine;
        pointOnLine_2 = pointOnLine + new Vector2(1, gradient);
        approachSide = true;
        approachSide = GetSide(pointPerpendicular);
    }

    bool GetSide(Vector2 p)
    {
        return (p.x-pointOnLine_1.x) * (pointOnLine_2.y-pointOnLine_2.y) > (p.y-pointOnLine_1.y) * (pointOnLine_2.x - pointOnLine_1.x);
    }

    public bool HasCrossedLine(Vector2 p)
    {
        return (GetSide(p) != approachSide);
    }

    public void DrawWithGizmos(float length)
    {
        Vector3 lineDir = new Vector3(1, gradient).normalized * length/2f;
        Vector3 lineCentre = new Vector3(pointOnLine_1.x, pointOnLine_1.y);
        Gizmos.DrawLine(lineCentre - lineDir, lineCentre + lineDir);
    }
}
