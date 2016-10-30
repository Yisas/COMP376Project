using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour
{
    [Header("--- Animation Variables ---")]
    [Tooltip("This float is communicated to the animator to set the speed of the tearing of an arm animation")]
    public float tearOffArmSpeed;

    [Header("--- References ---")]
    [Tooltip("A reference to this player's arm club gameobject")]
    public GameObject weaponArm;

    // State variables
    private int nbOfLimbs;
    private bool isDead;

    // References
    Animator anim;

	// Use this for initialization
	void Start ()
    {
        // Setup references
        anim = GetComponentInChildren<Animator>();

        // Setup variables
        nbOfLimbs = 3;
        
	}
	
	// Update is called once per frame
	void Update () {
	    if(nbOfLimbs <= 0)
        {
            Kill();
        }
	}

    // To be called when a player voluntarily rips off their own limb
    public void RipOffLimb()
    {
        switch (nbOfLimbs)
        {
            case 3:
                anim.SetFloat("takeOffArmSpeed", tearOffArmSpeed);
                anim.SetTrigger("takeOffArm");
                //weaponArm.SetActive(true);
                break;
            default:
                break;
        }
            
    }

    public void TakeOffLimb()
    {
        if(nbOfLimbs == 3)
        {
            //take off arm
            print("I took off arm");
            nbOfLimbs--;
        }

        else if(nbOfLimbs == 2)
        {
            //take off leg
            print("I took off leg");
            nbOfLimbs--;
        }

        else if(nbOfLimbs == 1)
        {
            //take off head
            print("I took off head");
            nbOfLimbs--;
        }
    }

    public void Kill()
    {
        if (!isDead)
        {
            print("I'm dead.");
            isDead = true;
        }
            
    }
}
