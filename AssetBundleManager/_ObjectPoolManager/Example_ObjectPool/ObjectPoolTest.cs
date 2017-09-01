using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolTest : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    List<GameObject> allObj = new List<GameObject>();

    public void CreatObjByPool()
    {
        GameObject go = ObjectPool.CreatGameObjByPool("Cube");
        allObj.Add(go);
        go.transform.SetParent(transform);
        go.transform.position = Vector3.zero;
    }

    public void DestroyByPool()
    {
        if (allObj.Count > 0)
        {
            ObjectPool.DestroyGameObjByPool(allObj[0]);
            allObj.RemoveAt(0);
        }
       
    }

}
