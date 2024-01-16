using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private float threshold = 0.01f;
    private Rigidbody2D rb;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(animator != null)
        {
            bool isMoving = rb.velocity.sqrMagnitude > threshold * threshold;
            if(!animator.GetBool("isMoving") && isMoving)
            {
                animator.SetBool("isMoving", true); 
            }
            else if(animator.GetBool("isMoving") && !isMoving)
            {
                animator.SetBool("isMoving", false);
            }
        }
    }
}
