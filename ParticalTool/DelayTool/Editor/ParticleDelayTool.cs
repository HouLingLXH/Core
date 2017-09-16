using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ParticleDelayTool : MonoBehaviour
{
    public int lastTime = 0;
    public int time = 0;
    public List<ParticleSystem> myParticleSystem = new List<ParticleSystem>(); 


    [ExecuteInEditMode]
    public void Add()
    {
        ChangeAllDelayTime(1);

    }



    //修改所有 延迟时间
    public void ChangeAllDelayTime(int add)
    {
        ParticleDelayTool[] allDelayTool = GetComponents<ParticleDelayTool>();

        for (int i = 0; i < allDelayTool.Length; i++)
        {
            allDelayTool[i].SaveSelfAndChild(add);
        }
    }

    //保存 当前延迟时间
    public void SaveSelfAndChild(int add)
    {
        for (int i = 0; i < myParticleSystem.Count; i++)
        {
            ParticleSystem[] l_particleSystemChild = myParticleSystem[i].transform.GetComponentsInChildren<ParticleSystem>();

            for (int k = 0; k < l_particleSystemChild.Length; k++)
            {
                l_particleSystemChild[k].startDelay += ((time - lastTime) * add);
            }
            myParticleSystem[i].startDelay = time;

            lastTime = time;


        }
    }

}

