using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game  {

    GameObject level;
    List<Vector3> posSpawn;
   
    bool endGame;

    // Use this for initialization
    public void Start (string levelName) {
		level = GameObject.Instantiate(Resources.Load("Prefabs/Level/"+levelName)) as GameObject;
        posSpawn = new List<Vector3>();
        GameObject spawn = GameObject.FindGameObjectWithTag("PosSpawn");

        for(int i = 0; i < spawn.transform.childCount; i++)
        {
            Transform go = spawn.transform.GetChild(i);
            posSpawn.Add(go.position);


            Player p = PlayerManager.Instance.getPlayer(i);
            if(p!= null)
                p.character.character.transform.position = go.position;
        }

        GameObject itemSPawn = GameObject.FindGameObjectWithTag("ItemSpawn");
        Vector2 pos1 = itemSPawn.transform.GetChild(0).position;
        Vector2 pos2 = itemSPawn.transform.GetChild(1).position;

        ItemManager.Instance.Start(pos1, pos2);


        SoundManager.Instance.Init();


    }

    // Update is called once per frame
    public void Update () {

        ItemManager.Instance.Update();

        int pAlive = PlayerManager.Instance.HowManyPlayersAlive();
        if (pAlive <= 1 && !endGame)
        {
            //ONE PLAYER WON
            if( pAlive == 1)
            {
                (FlowManager.Instance.getCurrentFlow() as GameFlow).EndGame();
            }
            else
            {

            }
        }

    
	}

    public Vector3 getRandomSpawnPos()
    {
        int rdm = Random.Range(0, posSpawn.Count);
        return posSpawn[rdm];
    }
}
