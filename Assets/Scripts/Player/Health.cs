using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour
{
    [Header("--- Animation Variables ---")]
    [Tooltip("This float is communicated to the animator to set the speed of the tearing of an arm animation")]
    public float tearOffArmSpeed;
    [Tooltip("This float is communicated to the animator to set the speed of the tearing of an arm animation")]
    public float tearOffLegSpeed;

    [Header("--- References ---")]
    [Tooltip("A reference to this player's arm club gameobject")]
    public GameObject weaponArm;
    [Tooltip("A reference to this player's regular foreground arm gameobject")]         // TODO: Consider switching this to a runtime search when I'm not lazy
    public GameObject foregroundArm;
    [Tooltip("A reference to this player's leg club gameobject")]
    public GameObject weaponLeg;
    [Tooltip("A reference to this player's regular foreground leg gameobject")]         // TODO: Consider switching this to a runtime search when I'm not lazy
    public GameObject foregroundLeg;

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
        nbOfLimbs = 2;
        
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
                break;
            case 2:
                anim.SetFloat("takeOffLegSpeed", tearOffLegSpeed);
                anim.SetTrigger("takeOffLeg");
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

    // To be called when the default, non weapon forground arm has to be replaced by the weapon arm object
    public void SwapArms()
    {
        // Deactivate whole thing IF NECESSARY THIS CAN BE CHANGED TO JUST SPIRTES AND COLLIDERS IN CASE WE NEED IT TO REMAIN ACTIVE FOR SEACHES, PROBABLY NOT HAPPENING
        foregroundArm.SetActive(false);

        // Reactivate weapon arm CONSIDER CLUB SPAWNING HERE AS A CHILD OF THE HAND INSTEAD OF REACTIVATING THE OBJECT
        weaponArm.SetActive(true);
    }

    // To be called when the default, non weapon forground leg has to be replaced by the weapon leg object
    public void SwapLegs()
    {
        // Deactivate whole thing IF NECESSARY THIS CAN BE CHANGED TO JUST SPIRTES AND COLLIDERS IN CASE WE NEED IT TO REMAIN ACTIVE FOR SEACHES, PROBABLY NOT HAPPENING
        foregroundLeg.SetActive(false);

        // Reactivate weapon leg CONSIDER CLUB SPAWNING HERE AS A CHILD OF THE HAND INSTEAD OF REACTIVATING THE OBJECT
        weaponLeg.SetActive(true);
    }

}
