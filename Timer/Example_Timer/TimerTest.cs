using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerTest : MonoBehaviour {

    public Text show;
	// Use this for initialization
	void Start () {
        //Play();

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Play()
    {
        //for (int i = 0; i < 5; i++)
        //{
            Debug.Log("now time:" + Time.timeSinceLevelLoad);
            Timer.DelayPlay(10, (o) =>
            {
                show.text = Time.timeSinceLevelLoad.ToString();
            });
        //}
       
    }


}
