using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPause
{
    const float VISIBLE = 1;
    const float INVISIBLE = 0.5f;

    GameObject pause;

    List<Image> choices;
        // Start is called before the first frame update
   public void Start()
    {
        pause = GameObject.Instantiate(Resources.Load("Prefabs/UI/Pause/PauseUI", typeof(GameObject))) as GameObject;
        GameObject choice = null;
        choices = new List<Image>();
        for(int i = 0; i < pause.transform.GetChild(0).childCount;i++)
        {
            if(pause.transform.GetChild(0).GetChild(i).tag == "UISelection")
            {
                choice = pause.transform.GetChild(0).GetChild(i).gameObject;
            }
        }

        for(int i = 0; i < choice.transform.childCount;i++)
        {
            choices.Add(choice.transform.GetChild(i).GetComponent<Image>());
        }


    }

    // Update is called once per frame
   public void UpdateUI(int c)
    {
        for(int i = 0; i < choices.Count; i++)
        {
            Color color = choices[i].color;
            if (c != i)
                color.a = INVISIBLE;
            else
                color.a = VISIBLE;

            choices[i].color = color;
        }
    }

    public int getNBChoices()
    {
        return choices.Count;
    }

    public void HideUI()
    {
        GameObject.Destroy(pause);
    }
}
