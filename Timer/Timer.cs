using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour {

    static List<TimerEvent> allDelayPlay ;
    static List<TimerEvent> clearList ;
    static List<TimerEvent> updateList;

    //单例
    public static GameObject go_Timer;
    public static Timer instance;

    //延时调用
    public static void DelayPlay(float time,TimerPlay timerPlay)
    {
        if (go_Timer == null)
        {
            go_Timer = new GameObject();
            go_Timer.name = "go_Timer";
            DontDestroyOnLoad(go_Timer);
            go_Timer.AddComponent<Timer>();
            allDelayPlay = new List<TimerEvent>();
            clearList = new List<TimerEvent>();
            updateList = new List<TimerEvent>();
        }

        TimerEvent timerEvent = new TimerEvent(timerPlay,time);

        allDelayPlay.Add(timerEvent);
    }

	//// Use this for initialization
	//void Start () {
		
	//}
	
	// Update is called once per frame
	void Update () {
        DisposeAllTimerEvent();
        ClearTimerEvent();
    }

    //处理所有timer
    void DisposeAllTimerEvent()
    {
        foreach (var item in allDelayPlay)
        {
            DisposeOneTimerEvent(item, item.time);
        }
    }

    //处理一条 timer
    void DisposeOneTimerEvent(TimerEvent timerEvent,float time)
    {
        time -= Time.deltaTime;
        if (time < 0)
        {
            timerEvent.play();
            clearList.Add(timerEvent);
        }
        else
        {
            updateList.Add(timerEvent);
        }
    }

    //清理已经完成的timer
    void ClearTimerEvent()
    {
        for (int i = 0; i < clearList.Count; i++)
        {
            allDelayPlay.Remove(clearList[i]);

        }
        clearList.Clear();

        for (int i = 0; i < updateList.Count; i++)
        {

            updateList[i].time -= Time.deltaTime;

        }
        updateList.Clear();
    }


}

public class TimerEvent
{
    public float time;
    public TimerPlay play;
    public TimerEvent(TimerPlay l_play,float l_time)
    {
        play = l_play;
        time = l_time;
    }
}


public delegate void TimerPlay(object[] param = null);