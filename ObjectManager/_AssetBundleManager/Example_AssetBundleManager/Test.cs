using UnityEngine;

public class Test : MonoBehaviour
{

    void Start()
    {
        AssetBundleManager.Init();
        LoadObjects();
    }

    void LoadObjects()
    {

        //GameObject go = Instantiate(AssetBundleManager.Load<GameObject>("cube","Cube"));
        //GameObject go2 = Instantiate(AssetBundleManager.Load<GameObject>("cube", "Cube"));
        //GameObject go2 = Instantiate(AssetBundleManager.Load("capsule"));
        //GameObject go3 = Instantiate(AssetBundleManager.Load("StreamingAssets"));

        for (int i = 0; i < 5; i++)
        {
            GameObject go3 = GameObjectManager.CreatGameObjectByPool("Cube");
        }
        



    }
}