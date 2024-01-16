using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerController : MonoBehaviour
{
    public Interactable interactable;
    public CircleCollider2D col;
    private void Awake()
    {
        interactable = GetComponentInParent<Interactable>();
        col = GetComponent<CircleCollider2D>();
        col.radius = interactable.distanceThreshold * 0.75f;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.TryGetComponent<Unit>(out Unit unit))
        {
            if(PlayerManager.instance.speakOnEnter && unit == PlayerManager.instance.player)
            {
                interactable.OpenText();
            }
        }
    }
}
