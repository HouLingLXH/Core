using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetManager : MonoBehaviour {


    static Dictionary<string, Object> m_allLoadedAsset = new Dictionary<string, Object>();//所有加载了的Asset 名称  此处就要求，所有asset 的名称必须唯一
    static Dictionary<Object, string> m_allLoadedAsset2 = new Dictionary<Object, string>();//所有加载了的Asset 名称 key 是Obj
    static Dictionary<int, bool> m_loadedAssetIsFree = new Dictionary<int, bool>();// 加载的Asset 是否是游离状态 key是 asset 的instanceID

    static Dictionary<Object, List<int>> m_allAssetBeUsedByObj = new Dictionary<Object, List<int>>(); //Aset -> Obj的使用情况 ，key 是 asset
    static Dictionary<int, Object> m_objUseAsset = new Dictionary<int, Object>();//  obj -> Asset 的信息

    static List<string> m_allNeedUnloadAssetName = new List<string>(); //所有准备卸载的资源名

    #region 提供外部使用的方法

    //初始化
    static public void Init()
    {
    }

    //从内存 卸载一个 资源的 引用标志,并卸载资源。
    static public void UnloadOneAsset(string assetName)
    {
        if (m_allLoadedAsset.ContainsKey(assetName))
        {
            Object asset = GetAssetByName(assetName);
            m_allLoadedAsset.Remove(assetName);
            UnloadOneAsset(asset);
        }
        else
        {
            Debug.LogError("m_allLoadedAsset 中不包含： " + assetName);
        }
    }



    //从内存 卸载一个 资源的 引用标志,并卸载资源。 (当该资源没有与bundle 断开连接时，不能用该方法删除，否则bundle 下次无法load该资源)
    static public void UnloadOneAsset(Object asset)
    {
        bool b_canRemove = false;
        string assetName = null;
        if (m_allLoadedAsset2.ContainsKey(asset))
        {
            assetName = GetNameByAsset(asset);
            if (m_allAssetBeUsedByObj.ContainsKey(asset))
            {
                if (m_allAssetBeUsedByObj[asset].Count < 1)
                {
                    if (IsFreeAsset(asset.GetInstanceID()))
                    {
                        b_canRemove = true;
                    }
                    else
                    {
                        Debug.LogWarning(assetName +"  ID:  "+ asset.GetInstanceID() + "  资源不是游离态,不能被卸载");
                    }
                    
                }
                else
                {
                    Debug.LogWarning(assetName + " 资源不能被卸载，引用数：" + m_allAssetBeUsedByObj[asset].Count);
                }
            }
            else
            {
                b_canRemove = true;
            }
        }

        else
        {
            Debug.LogError("m_allLoadedAsset2 中 不包含 " + asset);
        }
        if (b_canRemove)
        {
            m_allLoadedAsset2.Remove(asset);
            m_allLoadedAsset.Remove(assetName);
            Debug.Log("卸载资源 + " + asset);
            if (CanBeResourcesUnload(asset))
            {
                Resources.UnloadAsset(asset);
            }
            else
            {
                DestroyImmediate(asset, true);
            }
            

        }
       
    }

    //从内存 卸载所有不用的资源 
    static public void UnloadAllAsset()
    {
        m_allNeedUnloadAssetName.Clear();
        foreach (var item in m_allLoadedAsset)
        {
            m_allNeedUnloadAssetName.Add(item.Key);
        }
        for (int i = 0; i < m_allNeedUnloadAssetName.Count; i++)
        {
            UnloadOneAsset(m_allNeedUnloadAssetName[i]);
        }

    }

    //从内存 卸載自定义资源
    static public void UnloadSomeAsset(List<string> needUnload)
    {
        for (int i = 0; i < needUnload.Count; i++)
        {
            UnloadOneAsset(needUnload[i]);
        }
    }

    #endregion 

    #region 内部使用的方法
    //先读缓存，再从bundle中load
    static public T Load<T>(string assetName) where T : UnityEngine.Object
    {
        if (m_allLoadedAsset.ContainsKey(assetName))
        {

            return GetAssetByName(assetName) as T;
        }
        else
        {
            T asset = AssetBundleManager.Load<T>("cube", assetName); //以后要自动读取bundle名

            //对load的Asset进行记录
            RecordNewAsset(assetName, asset);

            return asset;
        }
    }

    //记录所有实例对 asset 的引用 //当 instance 新物体时使用
    static public void RecordObjUsedAsset(Object obj, Object asset)
    {
        int instanceID = obj.GetInstanceID();
        //记录 asset -> Obj
        if (m_allAssetBeUsedByObj.ContainsKey(asset))
        {
            if (m_allAssetBeUsedByObj[asset].Contains(instanceID))
            {
                Debug.LogError(asset + "重复记录实例：" + obj.name);
            }
            else
            {
                m_allAssetBeUsedByObj[asset].Add(instanceID);
            }
        }
        else
        {
            List<int> allObjID = new List<int>();
            allObjID.Add(instanceID);
            m_allAssetBeUsedByObj.Add(asset, allObjID);
        }


        //记录 Obj  ->  asset
        if (m_objUseAsset.ContainsKey(instanceID))
        {
            Debug.LogError(obj + "已经引用过其他资源了");
        }
        else
        {
            m_objUseAsset.Add(instanceID, asset);
        }


    }

    //移除某Obj 对asset 的引用
    static public bool RemoveObjUsedAsset(Object obj)
    {
        Object asset ;
        //移除 Obj -> Asset 的信息
        int instanceID = obj.GetInstanceID();
        if (m_objUseAsset.ContainsKey(instanceID))
        {
            asset = m_objUseAsset[instanceID];
            m_objUseAsset.Remove(instanceID);
        }
        else
        {
            Debug.LogError("实例：" + obj + "  ID:" + instanceID + " 没有记录他对Asset 的引用");
            return false;
        }

        //移除 Asset -> Obj 的信息

        if (m_allAssetBeUsedByObj.ContainsKey(asset))
        {
            if (m_allAssetBeUsedByObj[asset].Contains(instanceID))
            {
                m_allAssetBeUsedByObj[asset].Remove(instanceID);
                return true;
            }
            else
            {
                Debug.Log("资源： " + asset + "ID: " + asset.GetInstanceID() + "  没有对" + obj + "的使用记录");
            }
        }
        else
        {
            Debug.LogError("没有记录 实例" + obj + "对资源的引用");
        }
        return false;

    }


    //设置某资源为游离状态 （也就是其bundle unload（false）时，之后就可以通过asseManager的方法进行卸载了 ）
    static public void SetAssetFree(int assetInstanceID)
    {
        Debug.Log("设置资源 ： " + assetInstanceID + "为游离态");
        m_loadedAssetIsFree[assetInstanceID] = true;
    }


    #region 编辑器工具
    //获取所有已经加载的Asset 
    static public Dictionary<string, Object> GetAllLoadedAsset()
    {
        return new Dictionary<string, Object>(m_allLoadedAsset);
    }

    //获取所有已经加载的Asset2 
    static public Dictionary<Object, string> GetAllLoadedAsset2()
    {
        return new Dictionary<Object, string>(m_allLoadedAsset2);
    }

    //获取所有 asset 的引用情况
    static public Dictionary<Object,List<int>> GetUsedAssetByObj()
    {
        return new Dictionary<Object,List<int>>(m_allAssetBeUsedByObj);
    }

    //获取所有实例对象对 asset 的引用
    static public Dictionary<int, Object> GetObjUseAsset()
    {
        return new Dictionary<int, Object>(m_objUseAsset);
    }
    #endregion


    #region 类内工具
    //可以被Resources.Unload() 则返回true
    static private bool CanBeResourcesUnload(Object asset)
    {
        if (asset.GetType() == typeof(GameObject) ||
            asset.GetType() == typeof(Component)
            )
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    //初次加载，记录加载的asset，并且asset 不是游离态
    static private void RecordNewAsset(string assetName,Object asset)
    {
        m_allLoadedAsset.Add(assetName, asset);
        m_allLoadedAsset2.Add(asset, assetName);
        Debug.Log("新添加 assetName " + assetName);
        m_loadedAssetIsFree.Add(asset.GetInstanceID(), false);
    }


    //通过名称获取asset
    static private Object GetAssetByName(string assetName)
    {
        return m_allLoadedAsset[assetName];
    }

    //通过asset 获取其名称
    static private string GetNameByAsset(Object asset)
    {
        return m_allLoadedAsset2[asset];
    }

    //asset 是 游离状态
    static private bool IsFreeAsset(int assetInstanceID)
    {
        return m_loadedAssetIsFree[assetInstanceID];
    }
        
    #endregion
    #endregion



}
