using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public Camera cam;
    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Vector3 target = cam.ScreenToWorldPoint(Input.mousePosition);
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.GetRayIntersection(ray);
            if(hit.transform != null)
            {
                if(hit.transform.TryGetComponent<Interactable>(out Interactable interactable))
                {
                    interactable.OnHit();
                };
            }
            else
            {
                PlayerManager.instance.player.NavToTarget(target);
            }
        }
    }
}
