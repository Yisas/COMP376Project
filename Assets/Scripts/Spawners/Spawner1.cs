using UnityEngine;
using System.Collections;

public class Spawner1 : MonoBehaviour {

    public GameObject[] playerTemplates;
    public Transform[] spawnPoints;
    public FollowCam cam;

    GameObject newPlayer1;
    GameObject newPlayer2;

    PlayerController[] players;
    bool deadPlayer = false;

    // Use this for initialization
    void Start () {
        SpawnPlayers();
        players = FindObjectsOfType<PlayerController>();
    }
	
	// Update is called once per frame
	void Update () {
        CheckIfDead();
	}

    void CheckIfDead()
    {
        if (deadPlayer == false)
        {
            if (players[0] == null) //if player 2 is dead
            {
                int playerNum = players[0].playerNumber;
                StartCoroutine(SpawnPlayer(playerNum));
                deadPlayer = true;
            }
            if (players[1] == null) //if player 1 is dead
            {
                int playerNum = players[1].playerNumber;
                StartCoroutine(SpawnPlayer(playerNum));
                deadPlayer = true;
            }
        }
    }

    void SpawnPlayers()
    {
        bool dontSpawnP1 = false;
        bool dontSpawnP2 = false;
        PlayerController[] persistingPlayer = FindObjectsOfType<PlayerController>();
        if (persistingPlayer.Length == 1)
        {
            if (persistingPlayer[0].playerNumber == 1)
            {
                newPlayer1 = persistingPlayer[0].gameObject;
                newPlayer1.transform.position = spawnPoints[0].position;
                dontSpawnP1 = true;
            }
            else if (persistingPlayer[0].playerNumber == 2)
            {
                newPlayer2 = persistingPlayer[0].gameObject;
                newPlayer2.transform.position = spawnPoints[1].position;
                dontSpawnP2 = true;
            }
        }

        if (!dontSpawnP1) { 
        newPlayer1 = Instantiate(playerTemplates[0], spawnPoints[0].position, Quaternion.identity) as GameObject;
        Debug.Log("Player spawned");
        }
        if (!dontSpawnP2)
        {
            newPlayer2 = Instantiate(playerTemplates[1], spawnPoints[1].position, Quaternion.identity) as GameObject;
            Debug.Log("Player spawned");
        }
        spawnPoints[0].transform.position += new Vector3(-39, 0, 0);
        spawnPoints[1].transform.position += new Vector3(39, 0, 0);

        cam.Follow(newPlayer1.transform, newPlayer2.transform);
    }

    IEnumerator SpawnPlayer(int playerNum)
    {
        Debug.Log("Waiting to respawn player");

        yield return new WaitForSeconds(3f);
        if (playerNum == 1) //if player 1 is dead spawn him
        {
            newPlayer1 = Instantiate(playerTemplates[0], spawnPoints[0].position, Quaternion.identity) as GameObject;
        }
        else //if player 2 is dead spawn him
        {
            newPlayer2 = Instantiate(playerTemplates[1], spawnPoints[1].position, Quaternion.identity) as GameObject;
        }

        players = FindObjectsOfType<PlayerController>(); //reasign either player[0] or player[1] to not null
        cam.Follow(newPlayer1.transform, newPlayer2.transform);
        deadPlayer = false;
    }
}
