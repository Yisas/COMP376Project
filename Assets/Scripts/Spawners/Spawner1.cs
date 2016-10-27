using UnityEngine;
using System.Collections;

public class Spawner1 : MonoBehaviour {

    public GameObject[] players;
    public Transform[] spawnPoints;
    public FollowCam cam;

	// Use this for initialization
	void Start () {
        SpawnPlayers();
    }
	
	// Update is called once per frame
	void Update () {
        CheckIfDead();
	}

    void CheckIfDead()
    {

    }

    void SpawnPlayers()
    {

        /*for (int i = 0; i < players.Length && i < spawnPoints.Length; i++)
        {
            GameObject newPlayer = Instantiate(players[i], spawnPoints[i].position, Quaternion.identity) as GameObject;
            newPlayer.transform.parent = GameObject.Find("Players").transform;
            Debug.Log("Player spawned");
            
        }*/
        
        //When combat system is functionnal, uncomment use upper part. follow player should be handled by the combat sytem manager.
        GameObject newPlayer1 = Instantiate(players[0], spawnPoints[0].position, Quaternion.identity) as GameObject;
        newPlayer1.transform.parent = GameObject.Find("Players").transform;
        Debug.Log("Player spawned");

        GameObject newPlayer2 = Instantiate(players[1], spawnPoints[1].position, Quaternion.identity) as GameObject;
        newPlayer2.transform.parent = GameObject.Find("Players").transform;
        Debug.Log("Player spawned");

        cam.Follow(newPlayer1.transform, newPlayer2.transform);

    }
}
