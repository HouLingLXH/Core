using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSTool : MonoBehaviour {

    //是否输出fps
    private bool b_isDebug = false;

    // Use this for initialization
    void Start () {
        

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    //开关
    public void SetState(bool debug)
    {
        b_isDebug = debug;
    }

}
