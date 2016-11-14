using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    
    public int playerNumber;
	[Header("--- General Movement Variables ---")]
    public float moveSpeed;
    public float jumpForce;
	[Tooltip("This float is communicated to the animator to set the speed of the prepare for jump animation")]
	public float jumpPrepareSpeed;
	[Tooltip("This float is communicated to the animator to set the speed of the jump animation")]
	public float jumpSpeed;
	[Tooltip("This float is communicated to the animator to set the speed of the crouch animation")]
	public float crouchSpeed;
	[Tooltip("This float is communicated to the animator to set the speed of the dodge animation")]
	public float dodgeSpeed;
	[Tooltip("Single force to be applied on the dodge movement")]
	public float dodgeForce;

	[HideInInspector]				
    public bool animIsJabbing;
	[HideInInspector]
    public bool animIsClubbing;

    [Header("--- Attack Variables ---")]
	[Tooltip("This float is communicated to the animator to set the speed of the jab animation")]
    public float jabSpeed;
	[Tooltip("Minimum velocity treshold after which the player is considered running and the jab attack will be a dash")]
	public float jabDashTreshold;	
	[Tooltip("Single force to be applied on the dash attack")]
	public float jabDashForce;
    [Tooltip("Single force to be applied upon being punched, push player back")]
    public float jabStagger;
    [Tooltip("Single force to be applied upon being hit by weapon, push player back")]
    public float limbStagger;

    [Tooltip("Speed at witch the club animation prepares for the strike.")]
	public float clubAttackPepare;
	[Tooltip("Speed for the club attack animation.")]
	public float clubAttackSpeed;
	[Tooltip("Speed at witch the animation prepares for the strike.")]
	public float throwAttackPepare;
	[Tooltip("Speed for the animation.")]
	public float throwAttackSpeed;

	[Header("--- State Variables ---")]
    public LayerMask mWhatIsGround;
    public float kGroundCheckRadius = 0.1f;
    Vector3 direction;

    // Booleans
	bool inputLocked = false;		// Character controls will need to be locked for small intervals of time, like during a dash jab
	bool movementLocked = false;	// As above but just for the movement
    bool running;
	bool jumping;
    bool moving;
	bool dodging;
	bool crouching;					
    bool grounded;
    bool falling;
    bool attackingMelee;
    bool isHit;
    bool tearOffLimb;
    bool throwingLimb;

	public bool hasWeapon;			

	// Numerical variables
	private float moveInput = 0.0f;
	private int jabCounter = 0;


    // References
    Rigidbody2D rb;
	Transform myTransform;
    Transform sprites;
    Animator anim;
    Transform groundCheck;
    private Health health;
    public GameObject weaponArm;
    public GameObject weaponLeg;
    
	// Use this for initialization
	void Start ()
	{
        // Setup references
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        myTransform = transform;
        sprites = myTransform.FindChild("Sprites");
        groundCheck = transform.FindChild("GroundCheck");
	    health = GetComponent<Health>();

        if (playerNumber == 1)
            direction = new Vector2(1.0f, 0);
        if (playerNumber == 2)
            direction = new Vector2(-1.0f, 0);
	}
	
	// Update is called once per frame
	void Update ()
    {
        CheckGrounded();
        CheckFalling();
        ClampToCamera();

		if(!inputLocked)
        	CollectInput();

        if (attackingMelee)
        {
            MeleeAttack();
            attackingMelee = false;
        }
    }

    void FixedUpdate()
    {

        if(tearOffLimb)
        {
            health.RipOffLimb();			// Rip off limb returns true when the player succesfully equips a limb
            tearOffLimb = false;
        }

        Move();
		Crouch ();
		JumpPrepare();
        ThrowLimb();
		Dodge ();
    }

    private void CollectInput()
    {
		if (!movementLocked) 
			moveInput = Input.GetAxis ("Horizontal " + playerNumber);

        attackingMelee = Input.GetButtonDown("Melee Attack " + playerNumber);
		
		jumping = Input.GetButtonDown ("Jump" + playerNumber);

        tearOffLimb = Input.GetButtonDown("Tear Limb " + playerNumber);

		crouching = (Input.GetAxis ("Crouch " + playerNumber) == 0 ? false : true);

        throwingLimb = Input.GetButtonDown("Throw Limb " + playerNumber);

		dodging = Input.GetButtonDown ("Dodge " + playerNumber);
    }

    private void Move()
    {
		// Don't move while holding down crouch button. The crouch method cancels horizontal velocity
		if (!crouching) 
		{
			// Horizontal movement
		    if (moveInput != 0.0f)
		    {
                direction = new Vector2(moveInput, 0.0f);
            }
            //print("In the move function, the direction vector is: " + direction);
			myTransform.Translate (new Vector2(moveInput, 0.0f) * Time.deltaTime * moveSpeed);
			FaceDirection (direction);
			// Pass movement speed to animator
			anim.SetFloat ("speed", Mathf.Abs (moveInput));
		}
    }

	// The animator will finish the jump sequence after this method is called
    private void JumpPrepare()
    {
		if (grounded && jumping)
        {
			jumping = false;

			// Set animator speed variables and trigger attack type
			anim.SetFloat("jumpPrepareSpeed", jumpPrepareSpeed);
			anim.SetFloat("jumpSpeed", jumpSpeed);
			anim.SetTrigger ("jump");
        }
    }

	// The animator will call this method to apply the physics movement when the prepare animation is done
	public void JumpStart()
	{
		rb.velocity = new Vector2(0.0f, jumpForce);
	}


	private void Crouch()
	{
		if (crouching) 
		{
			CancelHorizontalVelocity ();

			anim.SetFloat ("crouchSpeed", crouchSpeed);
		}
		anim.SetBool ("crouching", crouching);
	}

    private void MeleeAttack()
	{
		// If the player doesn't have a weapon, jab attack...
		if (!hasWeapon) {
			// Read a speed treshold to see if you should do a dash attack
			if (Mathf.Abs (moveInput) >= jabDashTreshold) {
				// Lock movement inputs
				movementLocked = true;
				// Keep velocity in y, new velocity burst in x
				moveInput = 0;
				// Cancel prior horizontal velocity
				rb.velocity = new Vector2 (0, rb.velocity.y);
				// Using force instead of velocity to add single dash burst
				rb.AddForce (new Vector2 (jabDashForce * Mathf.Sign (direction.x), 0));
			}

			// Set animator speed variables and trigger attack type
			anim.SetFloat ("jabSpeed", jabSpeed);
			anim.SetTrigger ("jab");
            animIsJabbing = true;
        }
		// ... else use weapon as a club
		else 
		{
			// Set animator speed variables and trigger attack type
			anim.SetFloat ("clubPrepareSpeed",clubAttackPepare);
			anim.SetFloat ("clubSpeed",clubAttackSpeed);
			anim.SetTrigger ("club");
		    animIsClubbing = true;
		}
        
        
    }

	// Starts the throw animation. The once the animation is done it will call InstantiateAndThrowLimb
    private void ThrowLimb()
    {
        if (hasWeapon && throwingLimb)
        {
			anim.SetFloat ("throwLimbPrepareSpeed", throwAttackPepare);
			anim.SetFloat ("throwLimbSpeed", throwAttackSpeed);
			anim.SetTrigger ("throwLimb");
        }
    }

	private void Dodge()
	{
		if (dodging) 
		{
			// Add force in oposite horizontal direction to where the player is facing
			rb.AddForce (new Vector2(-Mathf.Sign(direction.x) * dodgeForce, direction.y));

			// Trigger animator
			anim.SetFloat ("dodgeSpeed", dodgeSpeed);
			anim.SetTrigger ("dodge");

			dodging = false;
		}
	}

	// Called by the animation to finish the attack
	public void InstantiateAndThrowLimb()
	{
		if (weaponArm)
		{
			print("Im throwing limb with direction: " + direction);
			Debug.Log ("Position of weapon arm was: " + weaponArm.transform.position);
			weaponArm.GetComponent<Throw>().ThrowLimb(direction);
		}

		else if (weaponLeg)
		{
			print("Im throwing limb with direction: " + direction);
			weaponLeg.GetComponent<Throw>().ThrowLimb(direction);
		}
		hasWeapon = false;
	}

    private void FaceDirection(Vector2 direction)
	{
        if (direction.x != 0.0f)
        {
            Quaternion rotation3D = direction.x > 0 ? Quaternion.LookRotation(Vector3.forward) : Quaternion.LookRotation(Vector3.back);
            sprites.rotation = rotation3D;
        }
    }

    private void CheckGrounded()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, kGroundCheckRadius, mWhatIsGround);
        foreach (Collider2D col in colliders)
        {
            if (col.gameObject != gameObject)
            {
                grounded = true;
				anim.SetBool ("grounded", grounded);
                return;
            }
        }
        grounded = false;

		anim.SetBool ("grounded", grounded);
    }

    private void CheckFalling()
    {
        falling = rb.velocity.y < 0.0f;
    }

    private void ClampToCamera()
    {
        Vector3 position = Camera.main.WorldToViewportPoint(myTransform.position);
        position.x = Mathf.Clamp(position.x, 0.03f, 0.97f);
       // position.y = Mathf.Clamp(position.y, 0.05f, 0.95f);
        myTransform.position = Camera.main.ViewportToWorldPoint(position);
    }

    public void GetHitByJab(GameObject oppositePlayer)
    {
        PlayerController opponent = oppositePlayer.GetComponent<PlayerController>();
        if (opponent)
        {
            if (opponent.animIsJabbing && !isHit) //if the opponent is in jab motion and I have not been hit yet
            {
                isHit = true; //I can't be hit twice by the same jab animation
                jabCounter++;
                rb.AddForce(jabStagger * (opponent.GetDirection())); // push player being hit back, "stagger"
                Debug.Log (jabCounter);
                if (jabCounter >= 3) //if I have been hit 3 times or more by a jab
                {
                    health.TakeOffLimb();
                    jabCounter = 0; //reset the jab counter
                }
            }
        }  
    }

    public void GetHitByClub(GameObject oppositePlayer)
    {
        PlayerController opponent = oppositePlayer.GetComponent<PlayerController>();
        if (opponent)
        {
            if (opponent.animIsClubbing && !isHit)
            {
                rb.AddForce(limbStagger * (opponent.GetDirection())); // push player being hit back, "stagger"
                StartCoroutine(RemoveLimb(0.5f));
            }
        }
    }

    public void GetHitByThrowingLimb()
    {
        health.Kill();
    }

    private IEnumerator RemoveLimb(float seconds)
    {
        isHit = true;
        health.TakeOffLimb();

        yield return new WaitForSeconds(seconds);

        isHit = false;
    }

	private void CancelHorizontalVelocity()
	{
		// Cancel horizontal velocities and stop running animations
		anim.SetFloat ("speed", 0);
		rb.velocity = new Vector2(0, rb.velocity.y);
	}

    public void SetIsHit(bool hit)
    {
        isHit = hit;
    }

    public bool GetIsHit()
    {
        return isHit;
    }
    
	public void SetInputLocked(bool locked)
	{
		inputLocked = locked;
	}

	public void SetMovementLocked(bool locked)
	{
		movementLocked = locked;
	}

    public Animator GetAnimator()
    {
        return anim;
    }

    public Vector3 GetDirection()
    {
        return direction;
    }

}
