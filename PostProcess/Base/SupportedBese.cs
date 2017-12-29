using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]//希望在编辑器模式下也能生效
[RequireComponent(typeof(Camera))]//自动关联camera 组件
public class SupportedBese : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        CanSupport(CheckSupport());

    }
    protected bool CheckSupport()
    {
        if (SystemInfo.supportsImageEffects == false) //如果显卡支持图像后期处理效果,返回true。
        {
            Debug.LogWarning("不支持图像后期效果 ");
            return false;
        }
        else
        {
            return true;
        }
    }

    protected void CanSupport(bool can)
    {
        this.enabled = can;
    }

    protected Material ChackShaderAndCreatMaterial(Shader shader, Material material = null)
    {
        if (shader == null || !shader.isSupported)//该shader是否支持后期特效
        {
            return null;
        }
        if (shader.isSupported && material != null && material.shader.name == shader.name)
        {
            return material;
        }
        else
        {
            material = new Material(shader);
            material.hideFlags = HideFlags.DontSave;
            if (material)
            {
       
                return material;
            }
            else
            {
                return null;
            }
        }
    }

}
