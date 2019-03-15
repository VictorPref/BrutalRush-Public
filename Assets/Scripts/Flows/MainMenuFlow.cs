using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuFlow : Flow {

    override
     public void InitializeFlow()
    {
        UiManager.Instance.StartMenu();
    }

    

    override
    public void UpdateFlow(float dt)
    {
        
      UiManager.Instance.UpdateMenu();
    }

    override
    public void FixedUpdateFlow(float dt)
    {

    }

    override
    public void CloseFlow()
    {
    }
}
