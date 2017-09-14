using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoolNum : MonoBehaviour {

    public Material rollMaterial;
    public float moveSpeed;
    float index;
    
    public float aimIndex; //目标位置
    public float step;//每个数字的高度

    // Use this for initialization
    void Start () {

        GetComponent<Image>().material = Instantiate(rollMaterial);
        rollMaterial = GetComponent<Image>().material;
        rollMaterial.SetFloat("_Step", step);
    }

    public void SetNum(int num )
    {
        aimIndex = num;
        index = num;
    }

    public void Move(int animNum, float l_moveSpeed)
    {
        moveSpeed = l_moveSpeed;
        aimIndex = animNum;
    }


    // Update is called once per frame
    void Update() {

        if (index == aimIndex || Mathf.Abs(aimIndex - index) < (0.01f))
        {
            index = aimIndex;
        }
        else
        {
            if (aimIndex > index)
            {
                index += Time.deltaTime * (aimIndex - index + 0.02f) * moveSpeed;
            }
            else
            {
                index -= Time.deltaTime * (index - aimIndex + 0.02f) * moveSpeed;
            }
        }
        rollMaterial.SetFloat("_ToNum", index);
    }
}
