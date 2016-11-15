using UnityEngine;
using System.Collections;

public class Spawner1 : MonoBehaviour {

    public GameObject[] players;
    public Transform[] spawnPoints;
    public FollowCam cam;

    GameObject newPlayer1;
    GameObject newPlayer2;

    PlayerController[] player;
    int numberOfPlayers;
    int playerNumber;
    bool deadPlayer = false;

    // Use this for initialization
    void Start () {
        SpawnPlayers();
        player = FindObjectsOfType<PlayerController>();

        numberOfPlayers = player.Length;
        Debug.Log(numberOfPlayers);
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
                playerNumber = 1;
                Debug.Log("Player 2 died so there is " + player.Length);
                StartCoroutine(SpawnPlayer2());
                deadPlayer = true;
            }
            if (player[1] == null) //if player 1 is dead
            {
                playerNumber = 2;
                Debug.Log("Player 1 died so there is " + player.Length);
                StartCoroutine(SpawnPlayer1());
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

    IEnumerator SpawnPlayer1()
    {
        Debug.Log("Waiting to respawn player1");

        yield return new WaitForSeconds(1.5f);
        newPlayer1 = Instantiate(players[0], spawnPoints[1].position, Quaternion.identity) as GameObject;
        player = FindObjectsOfType<PlayerController>();
        cam.Follow(newPlayer1.transform, newPlayer2.transform);
        deadPlayer = false;
    }

    IEnumerator SpawnPlayer2()
    {
        Debug.Log("Waiting to respawn player2");

        yield return new WaitForSeconds(1.5f);
        newPlayer2 = Instantiate(players[1], spawnPoints[0].position, Quaternion.identity) as GameObject;
        player = FindObjectsOfType<PlayerController>();
        cam.Follow(newPlayer1.transform, newPlayer2.transform);
        deadPlayer = false;
    }
}
