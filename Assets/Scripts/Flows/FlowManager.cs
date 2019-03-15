using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FlowManager
{
    //Name of the Scenes
    public enum SceneNames { MainMenu, Game,ShowRoom }

    #region Singleton
    private static FlowManager instance;

    private FlowManager() { }

    public static FlowManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new FlowManager();
            }
            return instance;
        }
    }
    #endregion
    public SceneNames currentScene;
    Flow currentFlow;
    bool flowInitialized = false;
    GamePackage packageGame;

    //Initialize FlowManager
    public void Initialize(SceneNames scene)
    {
        //Initialize first scene
        currentScene = scene;

        //Create Flow for the right scene
        currentFlow = CreateFlow(scene);
        flowInitialized = true;
        currentFlow.InitializeFlow();
        packageGame = new GamePackage();

    }

    //Update FlowManager
    public void Update(float dt)
    {

        //Update the flow
        if (currentFlow != null && flowInitialized)
            currentFlow.UpdateFlow(dt);
    }

    //FixedUpdate FlowManager
    public void FixedUpdate(float dt)
    {
        //Update the flow
        if (currentFlow != null && flowInitialized)
            currentFlow.FixedUpdateFlow(dt);
    }

    //Change the currentFlow
    public void ChangeFlows(SceneNames flowToLoad)
    {
        flowInitialized = false;
        //Close the currentFlow
        currentFlow.CloseFlow();
        //Create new flow
        currentFlow = CreateFlow(flowToLoad);


        //Add the function to the event
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadSceneAsync(flowToLoad.ToString());
    }

    private Flow CreateFlow(SceneNames flowToLoad)
    {
        Flow toRet = null;
        //Check the name of the scene to create the new flow
        switch (flowToLoad)
        {

            case SceneNames.MainMenu:
                toRet = new MainMenuFlow();
                break;
            case SceneNames.Game:
                toRet = new GameFlow();
                break;
            case SceneNames.ShowRoom:
                toRet = new GameFlow();
                break;
            default:
                Debug.Log("Unhandled switch " + flowToLoad);
                break;
        }
        return toRet;
    }

    //Function called by the event system SceneManager.sceneLoaded
    public void OnSceneLoaded(Scene sceneLoaded, LoadSceneMode loadScene)
    {
        //Initialize current flow
        currentFlow.InitializeFlow();
        flowInitialized = true;
        SceneManager.sceneLoaded -= OnSceneLoaded; //Clear the event system
    }
    public GamePackage GetGamePackage()
    {
        return packageGame;
    }

    public Flow getCurrentFlow()
    {
        return currentFlow;
    }
}
