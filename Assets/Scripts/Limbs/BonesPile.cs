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

    public void DestroyBonePile(BonesPile pile)
    {
        Destroy(pile);
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        GameObject playerGameobject = col.transform.root.gameObject;
        PlayerController player = playerGameobject.GetComponent<PlayerController>();
        Health health = playerGameobject.GetComponent<Health>();
        if (!player)
        {
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }

        else
        {
            if (player.hasWeapon == true && health.GetNbOfLimbs() == 3)
            {
                return;
            }
            else
            {
                Destroy(gameObject);
                health.PutBackLimb();
            }
                    
        }
    }
}
