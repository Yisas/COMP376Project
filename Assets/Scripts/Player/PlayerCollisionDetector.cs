﻿using UnityEngine;
using System.Collections;

public class PlayerCollisionDetector : MonoBehaviour
{
    private PlayerController player;

	// Use this for initialization
	void Start ()
	{
	    player = transform.root.gameObject.GetComponent<PlayerController>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    private IEnumerator Wait(float seconds)
    {
        yield return new WaitForSeconds(seconds);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        GameObject oppositePlayer = col.transform.root.gameObject;
        
        if (col.gameObject.CompareTag("Hand"))
        {
            if (gameObject != oppositePlayer)
                player.GetHitByJab(oppositePlayer);
        }

        else if (col.gameObject.CompareTag("Club"))
        {
            print(gameObject.name);
            print(col.gameObject.name);
            if(gameObject != oppositePlayer)
                player.GetHitByClub(oppositePlayer);
        }
        
    }

    void OnTriggerExit2D(Collider2D col)
    {
        StartCoroutine(Wait(0.75f));
        player.SetIsHit(false);
    }
}
