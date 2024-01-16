using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonTinter : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler

{
    public Text text;
    public Color color = Color.red;
    public float ratio = 0.1f;

    private void Awake()
    {
        text = GetComponent<Text>();
        text.color = color;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        ratio = Mathf.Clamp01(ratio);
        Color.RGBToHSV(color, out float h, out float s, out float v);
        s *= ratio;
        text.color = Color.HSVToRGB(h, s, v);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        text.color = color;
    }
}
