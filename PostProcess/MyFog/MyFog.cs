using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]//希望在编辑器模式下也能生效
[RequireComponent(typeof(Camera))]//自动关联camera 组件
public class MyFog : SupportedBese
{
    public Shader britSatConShader;
    private Material briSatConMaterial;

    public Material material {
        get {
            briSatConMaterial = ChackShaderAndCreatMaterial(britSatConShader, briSatConMaterial);
            return briSatConMaterial;
        }
    }

    [Range(0.0f, 3.0f)][Tooltip("亮度")]
    public float brightness = 1.0f;
    [Range(0.0f, 3.0f)][Tooltip("饱和度")]
    public float saturation = 1.0f;
    [Range(0.0f, 3.0f)][Tooltip("对比度")]
    public float contrast = 1.0f;



    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (material != null)
        {
            material.SetFloat("_Brightness", brightness);
            material.SetFloat("_Saturation", saturation);
            material.SetFloat("_Contrast", contrast);

            Graphics.Blit(source, destination, material);
        }
        else
        {
            Graphics.Blit(source, destination);
        }

    }

}
