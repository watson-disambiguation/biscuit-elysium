using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaderProcessor : MonoBehaviour
{
    public Shader CRT;
    public float curvature = 1.0f;
    public float vignetteWidth = 30.0f;
    private Material mat;
    [ExecuteInEditMode]
    private void Awake()
    {
        mat ??= new Material(CRT);
    }
    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        mat.SetFloat("_Curvature", curvature);
        mat.SetFloat("_VignetteWidth", vignetteWidth);
        Graphics.Blit(source, destination, mat);
    }
}
