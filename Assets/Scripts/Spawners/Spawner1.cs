using UnityEngine;
using System.Collections;

public class Spawner1 : MonoBehaviour {

    public GameObject[] players;
    public Transform[] spawnPoints;
    public FollowCam cam;

    GameObject newPlayer1;
    GameObject newPlayer2;

    private PlayerController[] player;
    private float timeOfDeath;
    private int playerNumber;
    private bool deadPlayer = false;

    // Use this for initialization
    void Start () {
        SpawnPlayers();
        player = FindObjectsOfType<PlayerController>();
    }
	
	// Update is called once per frame
	void Update () {
        CheckIfDead();
	}

    void CheckIfDead()
    {
        if (deadPlayer == false)
        {
            if (player[0] == null) //if player 2 is dead
            {
                timeOfDeath = Time.time;
                int playerNum = player[0].playerNumber;
                Debug.Log("Start timer");
                if(Time.time - timeOfDeath >= 1.5)
                {
                    Debug.Log("Timer started");
                    SpawnPlayer(playerNum);
                }
                //StartCoroutine(SpawnPlayer(playerNum));
                deadPlayer = true;
            }
            if (player[1] == null) //if player 1 is dead
            {
                timeOfDeath = Time.time;
                int playerNum = player[1].playerNumber;
                Debug.Log("Start Timer");
                if (Time.time - timeOfDeath >= 1.5)
                {
                    Debug.Log("Timer Started");
                    SpawnPlayer(playerNum);
                }
                // StartCoroutine(SpawnPlayer(playerNum));
                deadPlayer = true;
            }
        }
    }

    void SpawnPlayers()
    {

        /*for (int i = 0; i < players.Length && i < spawnPoints.Length; i++)
        {
            GameObject newPlayer = Instantiate(players[i], spawnPoints[i].position, Quaternion.identity) as GameObject;
            newPlayer.transform.parent = GameObject.Find("Players").transform;
            Debug.Log("Player spawned");
            
        }*/
        
        //When combat system is functionnal, use upper part. followPlayer should be handled by the combat sytem manager.
        newPlayer1 = Instantiate(players[0], spawnPoints[0].position, Quaternion.identity) as GameObject;
        Debug.Log("Player spawned");

        newPlayer2 = Instantiate(players[1], spawnPoints[1].position, Quaternion.identity) as GameObject;
        Debug.Log("Player spawned");

        cam.Follow(newPlayer1.transform, newPlayer2.transform);

    }

    private void SpawnPlayer(int playerNum)
    {
        playerNumber = playerNum;

        if (playerNumber == 1) //if player 1 is dead spawn him
        {
            newPlayer1 = Instantiate(players[0], spawnPoints[0].position, Quaternion.identity) as GameObject;
        }
        else if(playerNumber == 2) //if player 2 is dead spawn him
        {
            newPlayer2 = Instantiate(players[1], spawnPoints[1].position, Quaternion.identity) as GameObject;
        }

        cam.Follow(newPlayer1.transform, newPlayer2.transform);
        deadPlayer = false;
    }

    /*IEnumerator SpawnPlayer(int playerNum)
    {
        playerNumber = playerNum;
        Debug.Log("Waiting to respawn player");

        yield return new WaitForSeconds(1.5f);
        if (playerNumber == 1) //if player 1 is dead spawn him
        {
            newPlayer1 = Instantiate(players[0], spawnPoints[0].position, Quaternion.identity) as GameObject;
        }
        else //if player 2 is dead spawn him
        {
            newPlayer2 = Instantiate(players[1], spawnPoints[1].position, Quaternion.identity) as GameObject;
        }
        
        cam.Follow(newPlayer1.transform, newPlayer2.transform);
        deadPlayer = false;
    }*/
}
