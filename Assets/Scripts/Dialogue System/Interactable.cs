using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public float distanceThreshold = 2;
    public TextAsset inkJSON;
    public Outline highlight;

    void Start()
    {
        highlight.enabled = false;
    }
    public void OnHit()
    {
        Debug.Log("Hit");
        float distance = (PlayerManager.instance.player.transform.position - transform.position).magnitude;
        if(distance < distanceThreshold)
        {
            OpenText();
        }
        else
        {
            PlayerManager.instance.player.NavToTarget(this.transform.position);
            PlayerManager.instance.speakOnEnter = true;
        }
    }
    void OnMouseEnter()
    {
        if (!TextManager.instance.scrollList.activeSelf) highlight.enabled = true;
    }

    void OnMouseExit()
    {
        highlight.enabled = false;
    }

    public void OpenText()
    {
        PlayerManager.instance.speakOnEnter = false;
        TextManager.instance.scrollList.SetActive(true);
        TextManager.instance.Initialize(inkJSON);
        highlight.enabled=false;
    }


}
