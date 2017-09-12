using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBase : MonoBehaviour {


    // Update is called once per frame
    void Update () {
        OnUpdate();
    }

    public virtual void OnInit()
    {

    }

    public virtual void OnOpen()
    {

    }

    public virtual void OnClose()
    {
        GameObjectManager.DestroyGameObjectByPool(gameObject,false);
    }

    public virtual void OnUpdate()
    {

    }

}
