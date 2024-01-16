using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public bool canMove = true;
    public float lineLength;
    public float markerSize = 0.01f;
    public float speed = 5f;
    public float turnSpeed = 1f;
    public float turnDst = 1f;
    public Rigidbody2D rb;
    public float threshold = 0.05f;
    int targetIndex = 0;
    Vector3[] path;



    private Vector2 moveDir;

    public void NavToTarget(Vector3 target)
    {
        if (canMove) { PathRequestManager.RequestPath(transform.position, target, OnPathFound); }
        else { path = null; }
    }

    public void OnPathFound(Vector3[] newPath, bool pathSuccessful)
    {
        if (pathSuccessful)
        {
            path = newPath;
            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");

        }
    }

    IEnumerator FollowPath()
    {
        if (path.Length > 0)
        {
            targetIndex = 0;
            Vector3 currentWaypoint = path[0];
            while (canMove)
            {
                if ((transform.position - currentWaypoint).magnitude < threshold)
                {
                    targetIndex++;
                    if (targetIndex >= path.Length)
                    {
                        Debug.Log("Path Complete");
                        rb.velocity = Vector3.zero;
                        yield break;
                    }
                    currentWaypoint = path[targetIndex];
                }
                rb.velocity = (currentWaypoint - transform.position).normalized * speed;
                yield return null;
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (path != null)
        {
            for (int i = targetIndex; i < path.Length; i++)
            {
                Gizmos.color = Color.black;
                Gizmos.DrawCube(path[i], Vector3.one * 0.1f);

                if (i == targetIndex)
                {
                    Gizmos.DrawLine(transform.position, path[i]);
                }
                else
                {
                    Gizmos.DrawLine(path[i - 1], path[i]);
                }
            }
        }
    }
}
