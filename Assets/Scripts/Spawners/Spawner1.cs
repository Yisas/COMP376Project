using UnityEngine;
using System.Collections;

public class Spawner1 : MonoBehaviour {

    public GameObject player1;

	// Use this for initialization
	void Start () {
        SpawnPlayer();
    }
	
	// Update is called once per frame
	void Update () {
        CheckIfDead();
	}

    void CheckIfDead()
    {

    }

    void SpawnPlayer()
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(180, 158, 0));
        GameObject newPlayer1 = Instantiate(player1, ray.GetPoint(5), Quaternion.identity) as GameObject;
        newPlayer1.transform.parent = GameObject.Find("Players").transform;
        Debug.Log("Player spawned");
    }
}
