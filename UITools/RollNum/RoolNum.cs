using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoolNum : MonoBehaviour {

    public Material rollMaterial;
    public float moveSpeed;
    float index;
    
    public float aimIndex; //目标位置

    public float AimIndex
    {
        get
        {
            return aimIndex;
        }

        set
        {
            aimIndex = value;
        }
    }

    // Use this for initialization
    void Start () {

        GetComponent<Image>().material = Instantiate(rollMaterial);
        rollMaterial = GetComponent<Image>().material;
       
    }



    // Update is called once per frame
    void Update() {

        if (index == AimIndex || Mathf.Abs(AimIndex - index) < (0.03f))
        {
            index = AimIndex;
        }
        else
        {
            if (AimIndex > index)
            {
                index += Time.deltaTime * (AimIndex - index + 0.02f) * moveSpeed;
            }
            else
            {
                index -= Time.deltaTime * (index - AimIndex + 0.02f) * moveSpeed;
            }
        }
        rollMaterial.SetFloat("_ToNum", index);
    }
}
