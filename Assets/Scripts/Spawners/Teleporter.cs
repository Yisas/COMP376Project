using UnityEngine;
using System.Collections;

public class Teleporter : MonoBehaviour {

    public int teleporterNumber;
    float endTime = 5.5f; //this will have to be the same as respawn time in spawner script
    float timeRemaining = 2;
    bool playerTimer = true;

    PlayerController[] players;

    private bool teleport = false;

	// Use this for initialization
	void Start () {
       
	}
	
	// Update is called once per frame
	void Update () {

        timeRemaining -= Time.deltaTime;
        GetPlayers();
        CheckPlayers();
       
	}

    private void GetPlayers()
    {
        if (playerTimer == true)
        {
            if (timeRemaining <= 1)
            {
                players = FindObjectsOfType<PlayerController>();
                Debug.Log("Player[0]: " + players[0].playerNumber + " and Player[1]: " + players[1].playerNumber);
                playerTimer = false;
            }
        }
    }

    private void CheckPlayers()
    {
        if (timeRemaining <= 0)
        {
            if (players[0] == null || players[1] == null)
            {
                endTime -= Time.deltaTime;
                if (endTime <= 0)
                {
                    Debug.Log("Check player array");
                    players = FindObjectsOfType<PlayerController>();
                    Debug.Log("Player[0]: " + players[0].playerNumber + " and Player[1]: " + players[1].playerNumber);
                    endTime = 5.5f; //must be the same as the respawn time 
                }
            }
        }
    }

    private void ProgressToNextStage()
    {
        if (players[0] == null && teleporterNumber == 2) //player 2 dead
        {
            Debug.Log("Player 1 is trying to progress");
        }

        if(players[1] == null && teleporterNumber == 1) //player 1 dead
        {
            Debug.Log("Player 2 is trying to progress");
        }
    }

    void OnTriggerEnter2D (Collider2D collider)
    {
        GameObject obj = collider.gameObject;
        if (obj.GetComponent<PlayerCollisionDetector>())
        {
            ProgressToNextStage();
        }
    }
}
