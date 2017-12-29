using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FPSTool : MonoBehaviour {

    //是否输出fps
    private bool b_isDebug = false;

    public Text OutText;

    public float fpsMeasuringDelta = 2.0f;

    private float timePassed;
    private int m_FrameCount = 0;
    private float m_FPS = 0.0f;


    // Use this for initialization
    void Start () {
        SetState(ApplicationManager.B_openDebug);
        timePassed = 0.0f;

        float n_scale = Screen.width / 960f;
        transform.localScale = Vector3.one * n_scale;
    }
	
	// Update is called once per frame
	void Update () {
        if (b_isDebug == false)
            return;

        m_FrameCount = m_FrameCount + 1;
        timePassed = timePassed + Time.deltaTime;

        if (timePassed > fpsMeasuringDelta)
        {
            m_FPS = m_FrameCount / timePassed;

            timePassed = 0.0f;
            m_FrameCount = 0;
            OutText.text = m_FPS.ToString();
        }
        
    }

    //开关
    public void SetState(bool debug)
    {
        b_isDebug = debug;
    }

}
