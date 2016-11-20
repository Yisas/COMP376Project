﻿using UnityEngine;
using System.Collections;

public class Spawner1 : MonoBehaviour {

    public GameObject[] players;
    public Transform[] spawnPoints;
    public FollowCam cam;

    GameObject newPlayer1;
    GameObject newPlayer2;

    PlayerController[] player;
    int playerNumber;
    bool deadPlayer = false;

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
                int playerNum = player[0].playerNumber;
                StartCoroutine(SpawnPlayer(playerNum));
                deadPlayer = true;
            }
            if (player[1] == null) //if player 1 is dead
            {
                int playerNum = player[1].playerNumber;
                StartCoroutine(SpawnPlayer(playerNum));
                deadPlayer = true;
            }
        }
    }

    void SpawnPlayers()
    {
        newPlayer1 = Instantiate(players[0], spawnPoints[0].position, Quaternion.identity) as GameObject;
        Debug.Log("Player spawned");

        newPlayer2 = Instantiate(players[1], spawnPoints[1].position, Quaternion.identity) as GameObject;
        Debug.Log("Player spawned");

        spawnPoints[0].transform.position += new Vector3(-39, 0, 0);
        spawnPoints[1].transform.position += new Vector3(39, 0, 0);

        cam.Follow(newPlayer1.transform, newPlayer2.transform);
    }

    IEnumerator SpawnPlayer(int playerNum)
    {
        playerNumber = playerNum;
        Debug.Log("Waiting to respawn player");

        yield return new WaitForSeconds(5f);
        if (playerNumber == 1) //if player 1 is dead spawn him
        {
            newPlayer1 = Instantiate(players[0], spawnPoints[0].position, Quaternion.identity) as GameObject;
        }
        else //if player 2 is dead spawn him
        {
            newPlayer2 = Instantiate(players[1], spawnPoints[1].position, Quaternion.identity) as GameObject;
        }

        player = FindObjectsOfType<PlayerController>(); //reasign either player[0] or player[1] to not null
        cam.Follow(newPlayer1.transform, newPlayer2.transform);
        deadPlayer = false;
    }
}
