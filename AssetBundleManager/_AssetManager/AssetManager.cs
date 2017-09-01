using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetManager : MonoBehaviour {


    static Dictionary<string, Object> m_allLoadedAsset = new Dictionary<string, Object>();//所有加载了的Asset 名称  此处就要求，所有asset 的名称必须唯一
    static Dictionary<Object, List<int>> m_allAssetBeUsedObj = new Dictionary<Object, List<int>>(); //所有Asset的使用情况 ，key 是 asset
    static List<string> m_allNeedUnloadAssetName = new List<string>(); //所有准备卸载的资源名

    #region 提供外部使用的方法

    //初始化
    static public void Init()
    {
    }

    //从内存 卸载一个不用的资源
    static public void UnloadOneAsset(string assetName)
    {
        if (m_allLoadedAsset.ContainsKey(assetName))
        {
            Object asset = m_allLoadedAsset[assetName];
            if (m_allAssetBeUsedObj.ContainsKey(asset))
            {
                if (m_allAssetBeUsedObj[asset].Count < 1)
                {
                    m_allAssetBeUsedObj.Remove(asset);
                    m_allLoadedAsset.Remove(assetName);
                    Resources.UnloadAsset(asset);
                }
                else
                {
                    Debug.LogWarning(assetName + " 资源不能被卸载，引用数：" + m_allAssetBeUsedObj[asset].Count);
                }
            }
            else
            {
                Debug.LogError("m_allAssetBeUsedObj 中不包含资源： " + assetName + "   资源名：" + asset);
            }

        }
        else
        {
            Debug.LogError("m_allLoadedAsset 中不包含： " + assetName);
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
            return m_allLoadedAsset[assetName] as T;
        }
        else
        {
            T asset = AssetBundleManager.Load<T>("cube", assetName); //以后要自动读取bundle名
            m_allLoadedAsset.Add(assetName, asset);
            return asset;
        }
    }

    //记录所有实例对 asset 的引用 //当 instance 新物体时使用
    static public void RecordObjUsedAsset(Object obj, Object asset)
    {
        int instanceID = obj.GetInstanceID();
        if (m_allAssetBeUsedObj.ContainsKey(asset))
        {
            if (m_allAssetBeUsedObj[asset].Contains(instanceID))
            {
                m_allAssetBeUsedObj[asset].Add(instanceID);
            }
            else
            {
                Debug.LogError("重复记录实例：" + obj.name);
            }
        }
        else
        {
            List<int> allObjID = new List<int>();
            allObjID.Add(instanceID);
            m_allAssetBeUsedObj.Add(asset, allObjID);
        }
    }



    //获取所有已经加载的Asset 
    static public Dictionary<string, Object> GetAllLoadedAsset()
    {
        return new Dictionary<string, Object>(m_allLoadedAsset);
    }

    //获取所有实例对象对 asset 的引用
    static public Dictionary<Object,List<int>> GetAllObjUsedAsset()
    {
        return new Dictionary<Object,List<int>>(m_allAssetBeUsedObj);
    }
    #endregion



}
