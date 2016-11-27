using UnityEngine;
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
    

    void OnTriggerEnter2D(Collider2D col)
    {
        GameObject oppositePlayer = col.transform.root.gameObject;
        PlayerController opponent = oppositePlayer.GetComponent<PlayerController>();

		// Defensive programming, layer visibility should take care of the player hitting itself
        if (opponent)
        {
            if (opponent.playerNumber == player.playerNumber)
            {
                #if UNITY_EDITOR
                //Debug.Log ("Player " + player.playerNumber + " collided with itslef from PlayerCollisionDetection script attached to object " + gameObject.name + " and object " + col.gameObject.name + ". Aborting collision detection methods.");
                #endif
                return;
            }
        }
			

        if (col.gameObject.CompareTag("Hand"))
        {
            if (gameObject != oppositePlayer)
                player.GetHitByJab(oppositePlayer);
        }

        else if (col.gameObject.CompareTag("Club") && !player.GetIsHit())
        {
            Throw limb = col.gameObject.GetComponent<Throw>();

            if (limb != null && limb.LimbIsThrown() && player.playerNumber != limb.playerNumber)
            {
                Destroy(col.transform.parent.gameObject);
                player.GetHitByThrowingLimb();
            }

            else if (gameObject != oppositePlayer)
            {
                player.GetHitByClub(oppositePlayer);
            }

            
        }
        
    }

    void OnTriggerExit2D(Collider2D col)
    {
        GameObject oppositePlayer = col.transform.root.gameObject;

        // Defensive programming, layer visibility should take care of the player hitting itself
        if (oppositePlayer.GetComponent<PlayerController>() != null)
        {
            if (oppositePlayer.GetComponent<PlayerController>().playerNumber == player.playerNumber)
            {
                #if UNITY_EDITOR
                //Debug.Log ("Player " + player.playerNumber + " collided with itslef from PlayerCollisionDetection script attached to object " + gameObject.name + " and object " + col.gameObject.name + ". Aborting collision detection methods.");
                #endif
                return;
            }

            if (oppositePlayer.GetComponent<PlayerController>().animIsJabbing)
            {
                player.SetIsHit(false);
            }
        }

        
        
    }
}
