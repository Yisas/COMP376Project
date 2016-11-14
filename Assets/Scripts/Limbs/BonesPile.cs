using UnityEngine;
using System.Collections;

public class BonesPile : MonoBehaviour
{
    private Rigidbody2D rb;

	// Use this for initialization
	void Start ()
	{
	    rb = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnCollisionEnter2D(Collision2D col)
    {
        GameObject oppositePlayer = col.transform.root.gameObject;
        PlayerController player = oppositePlayer.GetComponent<PlayerController>();
        if (!player)
        {
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }

        else
        {
            
        }
    }
}
