using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticalHelper : MonoBehaviour {

    [SerializeField]
    ParticleSystem[] allParticle;

    [ContextMenu("查找节点下所有 ParticleSystem")]
    void FindAllPartical()
    {
        allParticle = transform.GetComponentsInChildren<ParticleSystem>();
        for (int i = 0; i < allParticle.Length; i++)
        {
            Debug.Log("找到" + allParticle[i].name);
        }
    }

    [ContextMenu("重播节点下所有 ParticleSystem")]
    //重新播放所有粒子特效
    public void RePlay()
    {
        for (int i = 0; i < allParticle.Length; i++)
        {
            allParticle[i].Play();
        }
    }

}
