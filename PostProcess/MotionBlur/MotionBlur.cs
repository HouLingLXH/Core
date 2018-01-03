using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotionBlur : SupportedBese {

    public Shader motionBlurShader;
    [Tooltip("混合系数")][Range(0,0.9f)]
    public float blend;
    private Material motionBlurMaterial;

    public RenderTexture lastTex = null;

    private void Awake()
    {
        //Application.targetFrameRate = 25; //限制帧率，但是要注意关掉垂直同步
    }

    private Material material
    {
        get {
            motionBlurMaterial = ChackShaderAndCreatMaterial(motionBlurShader, motionBlurMaterial);
            return motionBlurMaterial;
        }
        
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (material != null)
        {
            if (lastTex == null || lastTex.width != source.width || lastTex.height != source.height)
            {
                DestroyImmediate(lastTex);
                lastTex = new RenderTexture( source.width, source.height, 0);
                lastTex.filterMode = FilterMode.Bilinear;
                Graphics.Blit(source, lastTex);        
                
            }
            //lastTex.MarkRestoreExpected();//移动端代价昂贵，其实就是一个释放，可以自己管理
            material.SetFloat("_Blend", blend);

            Graphics.Blit(source, lastTex, material);
            Graphics.Blit(lastTex, destination);
            
        }
        else
        {
            Graphics.Blit(source, destination);
        }
    }

    private void OnDestroy()
    {
        DestroyImmediate(lastTex);
    }
}
