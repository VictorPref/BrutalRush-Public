using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIOption
{

    GameObject option;

    List<List<GameObject>> selection;
    List<Image> item;
    List<bool> active;
    Text nbLives;

    int options = 0;
    int choice = 0;

    bool leftStick = false;


    // Start is called before the first frame update
    public void Start()
    {
        option = GameObject.Instantiate(Resources.Load("Prefabs/UI/Option/Options", typeof(GameObject))) as GameObject;

        selection = new List<List<GameObject>>();
        item = new List<Image>();

        ItemManager.Instance.initActive();

        active = FlowManager.Instance.GetGamePackage().GetActiveItem();

        Transform panel = option.transform.GetChild(0);

        for (int i = 0; i < panel.childCount; i++)
        {
            Transform parent = null;

            if (panel.GetChild(i).tag == "UINbLife")
            {
                List<GameObject> s = new List<GameObject>();
                parent = panel.GetChild(i);

                for (int j = 0; j < parent.childCount; j++)
                {
                    if (parent.GetChild(j).tag == "UISelection")
                    {
                        s.Add(parent.GetChild(j).gameObject);

                    }
                    else if (parent.GetChild(j).tag == "UINbLife")
                    {
                        nbLives = parent.GetChild(j).GetComponent<Text>();
                    }
                }
                selection.Add(s);


            }

            if (panel.GetChild(i).tag == "item")
            {
                List<GameObject> s = new List<GameObject>();
                parent = panel.GetChild(i);
                Debug.Log(parent.name);

                for (int k = 0; k < parent.childCount; k++)
                {
                    Transform ch = parent.GetChild(k);
                    if (parent.GetChild(k).tag == "item")
                    {
                        for (int j = 0; j < ch.childCount; j++)
                        {
                            if (ch.GetChild(j).tag == "UISelection")
                            {
                                s.Add(ch.GetChild(j).gameObject);

                            }
                            else if (ch.GetChild(j).tag == "item")
                            {
                                item.Add(ch.GetChild(j).GetComponent<Image>());
                                active.Add(true);
                            }
                        }
                    }
                }
                selection.Add(s);
            }

        }
        Debug.Log(item.Count);
    }

    // Update is called once per frame
    public void Update()
    {
        InputManager.InputPkg pkg = InputManager.GetKeysInput(0);

        if (pkg.LeftStick.y > 0 && !leftStick)
        {
            SoundManager.Instance.Play("Button", SoundManager.SoundType.Menu);
            leftStick = true;
            options++;
            choice = 0;
            if (options > 1)
                options = 0;

        }
        else if (pkg.LeftStick.y < 0 && !leftStick)
        {
            SoundManager.Instance.Play("Button", SoundManager.SoundType.Menu);
            leftStick = true;
            options--;
            choice = 0;
            if (options < 0)
                options = 1;
        }
        if (pkg.LeftStick.x > 0 && !leftStick)
        {
            SoundManager.Instance.Play("Button", SoundManager.SoundType.Menu);
            leftStick = true;
            choice--;
            if (options == 0)
            {
                if (choice < 0)
                    choice = 1;
            }
            else
            {
                if (choice < 0)
                    choice = 4;
            }

        }
        else if (pkg.LeftStick.x < 0 && !leftStick)
        {
            SoundManager.Instance.Play("Button", SoundManager.SoundType.Menu);
            leftStick = true;
            choice++;

            if (options == 0)
            {
                if (choice > 1)
                    choice = 0;
            }
            else
            {
                if (choice > 4)
                    choice = 0;
            }

        }
        else if (pkg.LeftStick.y == 0 && pkg.LeftStick.x == 0)
        {
            SoundManager.Instance.Play("Button", SoundManager.SoundType.Menu);
            leftStick = false;
        }

        if (pkg.A)
        {
            ChangeOption();
        }


        UIUpdate();

        if (pkg.B)
        {
            SoundManager.Instance.Play("Button", SoundManager.SoundType.Menu);
            UiManager.Instance.getMenu().GoBackMenu();
        }
    }

    void ChangeOption()
    {
        if (options == 0)
        {
            if (choice == 0)
            {
                FlowManager.Instance.GetGamePackage().RemoveLives();
            }
            else if (choice == 1)
            {
                FlowManager.Instance.GetGamePackage().AddLives();

            }

        }
        else if (options == 1)
        {

            active[choice] = !active[choice];
            FlowManager.Instance.GetGamePackage().DesactiveActiveItem(active);

            ItemManager.Instance.setItemActive(active);
            ActiveDesactiveItem();
        }

        SoundManager.Instance.Play("Button", SoundManager.SoundType.Menu);
    }

    void UIUpdate()
    {
        for (int i = 0; i < selection.Count; i++)
        {
            for (int j = 0; j < selection[i].Count; j++)
            {
                if (j == choice && options == i)
                {
                    selection[i][j].SetActive(true);
                }
                else
                {
                    selection[i][j].SetActive(false);
                }
            }
        }


        ActiveDesactiveItem();
        nbLives.text = "" + FlowManager.Instance.GetGamePackage().GetNbLives();
    }

    void ActiveDesactiveItem()
    {
        GamePackage pkg = FlowManager.Instance.GetGamePackage();
        for (int i = 0; i < item.Count; i++)
        {
            Color c = item[i].color;
            if (active[i])
                c.a = 1;
            else
                c.a = 0.5f;

            item[i].color = c;
        }
    }

    public void removeUI()
    {
        GameObject.Destroy(option);
    }
}
