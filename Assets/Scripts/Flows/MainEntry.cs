using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainEntry : MonoBehaviour
{


    //Initialize flowManager
    public void Initialize()
    {
        GameObject.DontDestroyOnLoad(gameObject);
        FlowManager.Instance.Initialize((FlowManager.SceneNames)System.Enum.Parse(typeof(FlowManager.SceneNames), UnityEngine.SceneManagement.SceneManager.GetActiveScene().name));
    }

    //Update FlowManager
    public void Update()
    {
        FlowManager.Instance.Update(Time.deltaTime);
    }

    //FixedUpdate FlowManager
    public void FixedUpdate()
    {
        FlowManager.Instance.FixedUpdate(Time.fixedDeltaTime);
    }

}
