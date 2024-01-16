using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public bool canMove = true;
    public float speed = 5f;
    public Rigidbody2D rb;
    public float threshold = 0.05f;
    Vector3[] path;
    int targetIndex;
    [HideInInspector]
    public static Player instance;
    [HideInInspector]
    public bool speakOnEnter = false;


    private Vector2 moveDir;

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        if(rb == null)
        {
            rb = this.GetComponent<Rigidbody2D>();
        }
    }

    public void NavToTarget(Vector3 target)
    {
        speakOnEnter = false;
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
