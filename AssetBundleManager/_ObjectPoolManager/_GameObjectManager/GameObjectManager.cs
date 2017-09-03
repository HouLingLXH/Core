using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectManager : MonoBehaviour {

    static public void Init()
    {
        ObjectPool.Init();
        AssetBundleManager.Init();
    }

    //从对象池创建 gameObjec
    //因为active 会有大量时间消耗，默认不改变其active 状态
    //涉及partical 的重播，可以调用其ParticalHelper 的 RePlay() 方法
    static public GameObject CreatGameObjectByPool(string name, bool active = true)
    {
        return ObjectPool.CreatGameObjByPool(name,active);
    }

    //用对象池删除 gameObjec  
    //因为active 会有大量时间消耗，默认不改变其active 状态
    static public void DestroyGameObjectByPool(GameObject go, bool active = true)
    {
        ObjectPool.DestroyGameObjByPool(go,active);
    }

    //直接创建
    static public GameObject CreatGameObect(string name)
    {
        GameObject prafeb = AssetManager.Load<GameObject>(name);

        GameObject obj = Instantiate(prafeb);
        //记录 obj 与 Asset 间的关系
        AssetManager.RecordObjUsedAsset(obj, prafeb);
        obj.name = name;
        prafeb = null;
        return obj;
    }

    //直接删除
    static public void DestroyGameObject(GameObject go)
    {
        AssetManager.RemoveObjUsedAsset(go);
        Destroy(go);// go在下一帧才会释放
    }
}
