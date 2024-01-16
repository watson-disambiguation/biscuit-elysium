using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    float angle;
    Vector2 velocity;
    private void FixedUpdate()
    {
        velocity = PlayerManager.instance.player.rb.velocity;
        if(velocity != Vector2.zero)
        {
            angle = Vector2.Angle(Vector2.up, velocity);
            if (velocity.x < 0) angle = -angle;
            transform.localRotation = Quaternion.AngleAxis(angle, Vector3.up);
        }
    }
}
