using UnityEngine;
using System.Collections;

public class Spawner1 : MonoBehaviour {

    public GameObject[] playerTemplates;
    public Transform[] spawnPoints;
    public FollowCam cam;
    public LayerMask whatIsGround;
    static public int playerHeight = 30;

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

            for (int i = 0; i < players.Length; i++)
            {
                if (players[i] == null)
                {
                    int playerNum = players[i].playerNumber;
                    StartCoroutine(RespawnPlayer(playerNum));
                    deadPlayer = true;
                }
            }
        }
    }

    void SpawnPlayers()
    {
        PlayerController[] persistingPlayer = FindObjectsOfType<PlayerController>();

        bool dontSpawnP1 = false;
        bool dontSpawnP2 = false;
        Transform followP1 = null, followP2 = null;
        if (persistingPlayer.Length == 1)
        {
            if (persistingPlayer[0].playerNumber == 1)
            {
                newPlayer1 = persistingPlayer[0].gameObject;
                newPlayer1.transform.position = GetFloorSpawnPoint(1);
                followP1 = newPlayer1.transform;
                dontSpawnP1 = true;
            }
            else if (persistingPlayer[0].playerNumber == 2)
            {
                newPlayer2 = persistingPlayer[0].gameObject;
                newPlayer2.transform.position = GetFloorSpawnPoint(2);
                followP2 = newPlayer2.transform;
                dontSpawnP2 = true;
            }
        }


        
        if (playerTemplates[0] != null && spawnPoints[0] != null && !dontSpawnP1) {
            newPlayer1 = SpawnPlayer(1);
            followP1 = newPlayer1.transform;
            spawnPoints[0].transform.position += new Vector3(-39, 0, 0);
        }
        if (playerTemplates[1] != null && spawnPoints[1] != null && !dontSpawnP2)
        {
            newPlayer2 = SpawnPlayer(2);
            followP2 = newPlayer2.transform;
            spawnPoints[1].transform.position += new Vector3(39, 0, 0);
        }

        cam.Follow(followP1, followP2);
    }

    IEnumerator RespawnPlayer(int playerNum)
    {
        Debug.Log("Waiting to respawn player");

        yield return new WaitForSeconds(3f);
        if (playerNum == 1) //if player 1 is dead spawn him
        {
            newPlayer1 = SpawnPlayer(1);
        }
        else //if player 2 is dead spawn him
        {
            newPlayer2 = SpawnPlayer(2);
        }

        players = FindObjectsOfType<PlayerController>(); //reasign either player[0] or player[1] to not null
        cam.Follow(newPlayer1.transform, newPlayer2.transform);
        deadPlayer = false;
    }

    GameObject SpawnPlayer(int playerNum)
    {
        
        GameObject newPlayer = Instantiate(playerTemplates[playerNum-1], GetFloorSpawnPoint(playerNum) + new Vector2(0.0f, playerHeight), Quaternion.identity) as GameObject;

        Debug.Log("Player spawned");
        return newPlayer;
    }

    Vector2 GetFloorSpawnPoint(int playerNum)
    {
        RaycastHit2D hit = Physics2D.Raycast(spawnPoints[playerNum - 1].position, Vector2.down, 1000, whatIsGround);
        return hit.point;
    }
    
    
}
