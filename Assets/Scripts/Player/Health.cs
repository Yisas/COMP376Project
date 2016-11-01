﻿using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour
{
	[Header ("--- Animation Variables ---")]
	[Tooltip ("This float is communicated to the animator to set the speed of the tearing of an arm animation")]
	public float tearOffArmSpeed;
	[Tooltip ("This float is communicated to the animator to set the speed of the tearing of an arm animation")]
	public float tearOffLegSpeed;

	[Header ("--- References ---")]
	[Tooltip ("A reference to this player's arm club gameobject")]
	public GameObject weaponArm;
	[Tooltip ("A reference to this player's regular foreground arm gameobject")]         // TODO: Consider switching this to a runtime search when I'm not lazy
    public GameObject foregroundArm;
	[Tooltip ("A reference to this player's leg club gameobject")]
	public GameObject weaponLeg;
	[Tooltip ("A reference to this player's regular foreground leg gameobject")]         // TODO: Consider switching this to a runtime search when I'm not lazy
    public GameObject foregroundLeg;
    [Tooltip("A reference to this player's head to take it off when he's dead.")]
    public GameObject head;

	// State variables
	private int nbOfLimbs;
	private bool isDead;
    private PlayerController player;

	// References
	Animator anim;

	// Use this for initialization
	void Start ()
	{
		// Setup references
		anim = GetComponentInChildren<Animator> ();

		// Setup variables
		nbOfLimbs = 3;

	    player = GetComponent<PlayerController>();

	}
	
	// Update is called once per frame
	void Update ()
	{
		if (nbOfLimbs <= 0) {
			Kill ();
		}
	}

	// To be called when a player voluntarily rips off their own limb
	// Returns true if the player had a limb to equip and succesfully equips it. Otherwise it returns false
	public bool RipOffLimb ()
	{
	    if (!player.hasWeapon)
	    {
            switch (nbOfLimbs)
            {
                case 3:
                    anim.SetFloat("takeOffArmSpeed", tearOffArmSpeed);
                    anim.SetTrigger("takeOffArm");
                    player.hasWeapon = true;
                    return true;
                case 2:
                    anim.SetFloat("takeOffLegSpeed", tearOffLegSpeed);
                    anim.SetTrigger("takeOffLeg");
                    player.hasWeapon = true;
                    return true;
                default:
                    return false;
            }
        }

	    return false;
	}

	public void TakeOffLimb ()
	{
		if (nbOfLimbs == 3) {
            //take off arm
			foregroundArm.SetActive(false);
			print ("I took off arm");
			nbOfLimbs--;
		} else if (nbOfLimbs == 2) {
			//take off leg
            foregroundLeg.SetActive(false);
			print ("I took off leg");
			nbOfLimbs--;
		} else if (nbOfLimbs == 1) {
			//take off head
			print ("I took off head");
			nbOfLimbs--;
		}
	}

	public void Kill ()
	{
		if (!isDead) {
            gameObject.SetActive(false);
            print ("I'm dead.");
			isDead = true;
		}
            
	}

	// To be called when the default, non weapon forground arm has to be replaced by the weapon arm object
	public void SwapArms ()
	{
		// Deactivate whole thing IF NECESSARY THIS CAN BE CHANGED TO JUST SPIRTES AND COLLIDERS IN CASE WE NEED IT TO REMAIN ACTIVE FOR SEACHES, PROBABLY NOT HAPPENING
		foregroundArm.SetActive (false);

		// Reactivate weapon arm CONSIDER CLUB SPAWNING HERE AS A CHILD OF THE HAND INSTEAD OF REACTIVATING THE OBJECT
		weaponArm.SetActive (true);
	}

	// To be called when the default, non weapon forground leg has to be replaced by the weapon leg object
	public void SwapLegs ()
	{
		// Deactivate whole thing IF NECESSARY THIS CAN BE CHANGED TO JUST SPIRTES AND COLLIDERS IN CASE WE NEED IT TO REMAIN ACTIVE FOR SEACHES, PROBABLY NOT HAPPENING
		foregroundLeg.SetActive (false);

		// Reactivate weapon leg CONSIDER CLUB SPAWNING HERE AS A CHILD OF THE HAND INSTEAD OF REACTIVATING THE OBJECT
		weaponLeg.SetActive (true);
	}

}
