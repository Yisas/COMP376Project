using UnityEngine;
using System.Collections;

// For comunication between the animator controller and the PlayerController, since they have to be in separate objects
public class AuxAnimationCommunicator : MonoBehaviour {

	PlayerController playerController;

	void Start()
	{
		playerController = GetComponentInParent<PlayerController> ();
	}

	// Using integer notation for input since the animator function called doesn't support bool
	public void SetInputLocked(int locked)
	{
		if(locked == 0)
			playerController.SetInputLocked (false);
		else if(locked==1)
			playerController.SetInputLocked (true);
	}

	// Using integer notation for input since the animator function called doesn't support bool
	public void SetMovementLocked(int locked)
	{
		if(locked == 0)
			playerController.SetMovementLocked (false);
		else if(locked==1)
			playerController.SetMovementLocked (true);
	}

    public void DisableIsAttacking()
    {
        playerController.animIsJabbing = false;
    }

	public void JumpStart()
	{
		playerController.JumpStart ();
	}
}
