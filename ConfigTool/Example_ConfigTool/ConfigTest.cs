using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfigTest : MonoBehaviour {

    public SerializableSet set;

    // Use this for initialization
    void Start()
    {
        Deserializer.Deserialize(set);

    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(PlayerDataConfig.Get(3).name);
    }
}
