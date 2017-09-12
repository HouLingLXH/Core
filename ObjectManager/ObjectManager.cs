using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour {

    static private Object[] allObject; //工程中所有Object

    //更新所有Object
    static public void UpdateAllObject()
    {
        ClearAllObject();
        Resources.UnloadUnusedAssets();
        allObject = Resources.FindObjectsOfTypeAll(typeof(Object));
    }

    //获取所有当前项目中的Object
    static public Object[] GetAllObject()
    {
        return allObject;
    }

    //防止因为其中引用，而不能卸载无用的资源
    static public void ClearAllObject()
    {
        allObject = null;
    }

    //获取某种类型的Obj
    static public List<Object> GetSomeObject<T> () where T : Object 
    {
        List<Object> someObject = new List<Object>( Resources.FindObjectsOfTypeAll(typeof(T)));


        return someObject;
    }


    //通过Instance ID 查找 Obj
    static public Object GetObjectFromID(int id)
    {
        UpdateAllObject();
        foreach (Object o in allObject)
        {
            if (o.GetInstanceID() == id)
            {
                //Debug.Log("找到 Object： " + id);
                return o;
            }
        }
        //Debug.Log("没找到 Object： " + id);
        return null;
    }

}
