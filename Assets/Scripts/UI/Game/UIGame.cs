using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGame
{
    GameObject uiGame;
    List<GameObject> uiPlayer;
    List<Image> healthBar;
    List<Image> energyBar;
    List<Text> nbLife;
    List<Image> imagePlayer;
    List<GameObject> specialEnergyBar;
    Material black;

    // Start is called before the first frame update
    public void Start()
    {
        uiGame = GameObject.FindGameObjectWithTag("UIGame");
        black = Resources.Load("Materials/Black", typeof(Material)) as Material;
        uiPlayer = new List<GameObject>();
        imagePlayer = new List<Image>();
        specialEnergyBar = new List<GameObject>();

        for (int i = 0; i < PlayerManager.Instance.maxPlayer; i++)
        {
            GameObject uiP = GameObject.FindGameObjectWithTag("P" + (i + 1));


            if (PlayerManager.Instance.getPlayer(i) == null)
            {
                uiP.SetActive(false);
            }
            else
            {
                uiPlayer.Add(uiP);
            }

        }
        healthBar = new List<Image>();
        energyBar = new List<Image>();
        nbLife = new List<Text>();

        for (int i = 0; i < uiPlayer.Count; i++)
        {
            int childCount = uiPlayer[i].transform.childCount;

            for (int j = 0; j < childCount; j++)
            {
                GameObject child = uiPlayer[i].transform.GetChild(j).gameObject;


                if (child.tag == "UIPlayerImage")
                {
                    imagePlayer.Add(child.GetComponent<Image>());
                }

                if (child.tag == "UINumPlayer")
                {
                    PlayerManager.Instance.SetUpClampNamePlayer(child.GetComponent<Text>(), i);
                }

                if (child.tag == "UIInfoPlayer")
                {
                    int childCount2 = child.transform.childCount;

                    for (int k = 0; k < childCount2; k++)
                    {
                        GameObject child1 = child.transform.GetChild(k).gameObject;

                        if (child1.tag == "UINbLife")
                        {
                            nbLife.Add(child1.GetComponent<Text>());
                        }
                        else
                        {
                            int childCount3 = child1.transform.childCount;

                            for (int h = 0; h < childCount3; h++)
                            {

                                GameObject child2 = child1.transform.GetChild(h).gameObject;

                                if (child2.tag == "UILifeBar")
                                {
                                    healthBar.Add(child2.GetComponent<Image>());
                                }
                                if (child2.tag == "UIEnergyBar")
                                {
                                    energyBar.Add(child2.GetComponent<Image>());
                                }

                                if (child2.tag == "SpecialBar")
                                {
                                    specialEnergyBar.Add(child2);
                                }

                            }

                        }


                    }


                }
            }
        }

    }

    public void ChangeImageSource(Color color, int id)
    {
        imagePlayer[id].color = color;
    }

    public void UpdateUiPlayer(int id, float health, float energy, int nbLives)
    {

        healthBar[id].fillAmount = health / Character.MAX_HEALTH;

        if (energy > 5.1f)
        {
            Debug.Log(black);
            energyBar[id].material = black;
        }
        else
        {
            energyBar[id].material = null;
        }
        energyBar[id].fillAmount = energy / Character.MAX_ENERGY;
        nbLife[id].text = "x" + nbLives;

    }

    public void ActiveDesactiveBar(int id, bool state)
    {
        specialEnergyBar[id].SetActive(state);
    }

    // Update is called once per frame
    public void Update()
    {

    }
}
