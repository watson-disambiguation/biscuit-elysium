using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path
{
    public readonly Vector3[] lookPoints;
    public readonly Line[] turnBoundaries;
    public readonly int finishLineIndex;

    public Path(Vector3[] waypoints, Vector3 startPos, float turnDst)
    {
        lookPoints = waypoints;
        turnBoundaries = new Line[waypoints.Length];
        finishLineIndex = turnBoundaries.Length-1;

        Vector2 previousPoint = startPos;
        for (int i = 0; i < lookPoints.Length; i++)
        {
            Vector2 currentPoint = lookPoints[i];
            Vector2 dirToCurrentPoint = (currentPoint - previousPoint).normalized;
            Vector2 turnBoundaryPoint = (i == finishLineIndex) ? currentPoint : (currentPoint - dirToCurrentPoint * turnDst);
            turnBoundaries[i] = new Line(turnBoundaryPoint, previousPoint);
        }
    }

    public void DrawWithGizmos(float length, float size)
    {
        Gizmos.color = Color.black;
        for (int i = 1; i < lookPoints.Length; i++)
        {
            Vector2 pCur = lookPoints[i];
            Vector2 pPrev = lookPoints[i-1];
            Gizmos.DrawCube(new Vector3(pCur.x, pCur.y), Vector3.one * size);
            Gizmos.DrawLine(pPrev, pCur);
        }
        Gizmos.color = Color.white;
        foreach (Line l in turnBoundaries)
        {
            l.DrawWithGizmos(length);
        }
    }
}
