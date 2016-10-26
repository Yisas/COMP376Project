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
	[HideInInspector]				
    public bool animIsJabbing;
    public bool animIsClubbing;

    [Header("--- Attack Variables ---")]
	[Tooltip("This float is communicated to the animator to set the speed of the jab animation")]
    public float jabSpeed;
	[Tooltip("Minimum velocity treshold after which the player is considered running and the jab attack will be a dash")]
	public float jabDashTreshold;	
	[Tooltip("Single force to be applied on the dash attack")]
	public float jabDashForce;

	[Tooltip("Speed at witch the club animation prepares for the strike.")]
	public float clubAttackPepare;
	[Tooltip("Speed for the club attack animation.")]
	public float clubAttackSpeed;

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
    bool grounded;
    bool falling;
    bool attackingMelee;
    bool isHit;
	[Header("This is temporarily public until we add the functionality to modify at runtime.")]
	public bool hasWeapon;			

	// Numerical variables
	private float moveInput = 0.0f;
	private float jabCounter = 0.0f;


    // References
    Rigidbody2D rb;
	Transform transform;
    Transform sprites;
    Animator anim;
    Transform groundCheck;
    private Health health;
    
	// Use this for initialization
	void Start ()
	{
        // Setup references
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        transform = GetComponent<Transform>();
        sprites = transform.FindChild("Sprites");
        groundCheck = transform.FindChild("GroundCheck");
	    health = GetComponent<Health>();

	}
	
	// Update is called once per frame
	void Update ()
    {
        CheckGrounded();
        CheckFalling();

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
        Move();
		JumpPrepare();
    }

    private void CollectInput()
    {
        if (!movementLocked)
            moveInput = Input.GetAxis("Horizontal " + playerNumber);

        attackingMelee = Input.GetButtonDown("Melee Attack " + playerNumber);
		
		jumping = Input.GetButtonDown ("Jump" + playerNumber);
    }

    private void Move()
    {
        // Horizontal movement
        direction = new Vector2(moveInput, 0.0f);
        transform.Translate(direction * Time.deltaTime * moveSpeed);
        FaceDirection (direction);
        // Pass movement speed to animator
        anim.SetFloat("speed",Mathf.Abs(moveInput));
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
			anim.SetFloat ("clubPrepareSpeed",clubAttackSpeed);
			anim.SetFloat ("clubSpeed",clubAttackPepare);
			anim.SetTrigger ("club");
		    animIsClubbing = true;
		}
        
        
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

    public void GetHitByJab(GameObject oppositePlayer)
    {
        PlayerController opponent = oppositePlayer.GetComponent<PlayerController>();
        if (opponent)
        {
            if (opponent.animIsJabbing && !isHit) //if the opponent is in jab motion and I have not been hit yet
            {
                isHit = true; //I can't be hit twice by the same jab animation
                jabCounter++;
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
                isHit = true;
                health.TakeOffLimb();
            }
        }
    }

    public void SetIsHit(bool hit)
    {
        isHit = hit;
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

}
