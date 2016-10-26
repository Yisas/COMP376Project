using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    
    public int playerNumber;
	[Header("--- General Movement Variables ---")]
    public float moveSpeed;
    public float jumpForce;
    public bool animIsJabbing;

    [Header("--- Attack Variables ---")]
	[Tooltip("This float is communicated to the animator to set the speed of the jab animation")]
    public float jabSpeed;
	[Tooltip("Minimum velocity treshold after which the player is considered running and the jab attack will be a dash")]
	public float jabDashTreshold;	
	[Tooltip("Single force to be applied on the dash attack")]
	public float jabDashForce;

	[Header("--- State Variables ---")]
    public LayerMask mWhatIsGround;
    public float kGroundCheckRadius = 0.1f;
    Vector3 direction;

    // Booleans
	bool inputLocked = false;		// Character controls will need to be locked for small intervals of time, like during a dash jab
	bool movementLocked = false;	// As above but just for the movement
    bool running;
    bool moving;
    bool grounded;
    bool falling;
    bool attackingMelee;
    bool isHit;
                         

    // References
    Rigidbody2D rb;
	Transform transform;
    Transform sprites;
    Animator anim;
    Transform groundCheck;

    private float moveInput = 0.0f;
    private float jabCounter = 0.0f;
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
        Jump();
    }

    private void CollectInput()
    {
        if (!movementLocked)
            moveInput = Input.GetAxis("Horizontal " + playerNumber);

        attackingMelee = Input.GetButtonDown("Melee Attack " + playerNumber);
		
        
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

    private void Jump()
    {
        if (grounded && Input.GetButton("Jump"))
        {
            rb.velocity = new Vector2(0.0f, jumpForce);
        }
    }

    private void MeleeAttack()
    {
		// Read a speed treshold to see if you should do a dash attack
		if (Mathf.Abs (moveInput) >= jabDashTreshold) 
		{
			// Lock movement inputs
			movementLocked = true;
			// Keep velocity in y, new velocity burst in x
			moveInput = 0;
			// Cancel prior horizontal velocity
			rb.velocity = new Vector2 (0, rb.velocity.y);
			// Using force instead of velocity to add single dash burst
			rb.AddForce (new Vector2( jabDashForce * Mathf.Sign(direction.x), 0));
		}

        anim.SetFloat("jabSpeed",jabSpeed);
        //anim.SetTrigger("jab");
        anim.SetBool("isAttacking", true);
        animIsJabbing = true;
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
                return;
            }
        }
        grounded = false;
    }

    private void CheckFalling()
    {
        falling = rb.velocity.y < 0.0f;
    }

    public void RemoveHealth(GameObject oppositePlayer)
    {
        PlayerController opponent = oppositePlayer.GetComponent<PlayerController>();
        if (opponent)
        {
            if (opponent.animIsJabbing && !isHit) //if the opponent is in jab motion and I have not been hit yet
            {
                isHit = true; //I can't be hit twice by the same jab animation
                jabCounter++;
                if (jabCounter >= 3) //if I have been hit 3 times or more by a jab
                {
                    health.TakeOffLimb();
                    jabCounter = 0; //reset the jab counter
                }
            }
        }  
    }

    public void SetIsHit(bool hit)
    {
        isHit = hit;
    }

    public bool IsHit()
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

}
