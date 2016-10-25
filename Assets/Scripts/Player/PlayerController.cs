using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    public float moveSpeed;
    public float jumpForce;
    public float jabSpeed;
	public float jabDashTreshold;	// Minimum velocity treshold after which the player is considered running and the jab attack will be a dash
	public float jabDashForce;		// Single force to be applied on the dash attack
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

    // References
    Rigidbody2D rb;
	Transform transform;
    Transform sprites;
    Animator anim;
    Transform groundCheck;

    private float moveInput = 0.0f;

	// Use this for initialization
	void Start ()
    {
        // Setup references
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        transform = GetComponent<Transform>();
        sprites = transform.FindChild("Sprites");
        groundCheck = transform.FindChild("GroundCheck");
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
		// Movement Inputs
		if (!movementLocked) 
		{
			moveInput = Input.GetAxis ("Horizontal");
		}

		// Attack Inputs
        attackingMelee = Input.GetButtonDown("Melee Attack");
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
        anim.SetTrigger("jab");
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
    
	public void SetInputLocked(bool locked)
	{
		inputLocked = locked;
	}

	public void SetMovementLocked(bool locked)
	{
		movementLocked = locked;
	}
}
