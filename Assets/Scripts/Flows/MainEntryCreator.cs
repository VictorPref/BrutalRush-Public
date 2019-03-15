using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainEntryCreator : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        //if there's no game object of type MainEntry create a new one and add the script to it
        if (!GameObject.FindObjectOfType<MainEntry>())
        {
            GameObject mainFlowObj = new GameObject();
            mainFlowObj.name = "MainEntry";
            mainFlowObj.AddComponent<MainEntry>().Initialize();
        }
        //destroy itself
        Destroy(gameObject);
    }
}
