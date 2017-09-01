using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectManagerTest : MonoBehaviour {

    public GameObject cube;
	// Use this for initialization
	void Start () {
        cube = GameObjectManager.CreatGameObect("Cube");
    }
	
	// Update is called once per frame
	void Update () {
        //TestSetActivePrice();
    }

    //测试证明，当go.active == true 时，再次调用SetActive(true)时，无消耗
    void TestSetActivePrice()
    {
        Debug.Log(Time.timeSinceLevelLoad);
        for (int i = 0; i < 10000; i++)
        {
            cube.SetActive(true);
            cube.SetActive(false);
        }
        Debug.Log(Time.timeSinceLevelLoad);
    }


}
