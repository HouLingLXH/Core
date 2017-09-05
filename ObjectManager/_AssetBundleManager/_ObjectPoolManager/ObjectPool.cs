using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour {


    static Dictionary<string, List<GameObject>> PoolObjects; //所有池中对象

    static GameObject ObjectPool_go;//池
    static Transform ObjectPool_tran;//池 
    //初始化
    public static void Init()
    {
        if (ObjectPool_go == null)
        {
            ObjectPool_go = new GameObject();
            ObjectPool_go.name = "ObjectPool";
            ObjectPool_tran = ObjectPool_go.transform;
            DontDestroyOnLoad(ObjectPool_go);
            PoolObjects = new Dictionary<string, List<GameObject>>();
            AssetBundleManager.Init();
        }
    }

    //创建
    public static GameObject CreatGameObjByPool(string name, bool active = true)
    {
        Init();
        GameObject obj = null;
        if (PoolObjects.ContainsKey(name))
        {
            if (PoolObjects[name].Count > 0)
            {
                obj = PoolObjects[name][0];
                PoolObjects[name].RemoveAt(0);
            }
        }
        if (obj == null)
        {
            obj = GameObjectManager.CreatGameObect(name);
        }

        obj.SetActive(active);

        return obj;
    }

    //入池物体所在位置中心点
    static Vector3 PoolPos = new Vector3(10000, 10000, 10000);
    //入池
    public static void DestroyGameObjByPool(GameObject go,bool active = false)
    {
        Init();
        string goName = go.name;
        if (!PoolObjects.ContainsKey(goName))
        {
            PoolObjects.Add(goName, new List<GameObject>());
        }
        go.transform.SetParent(ObjectPool_tran);
        go.transform.position = go.transform.position +  PoolPos;
        
        go.SetActive(active);
        PoolObjects[goName].Add(go);
    }
}
