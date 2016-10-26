using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    
    public int playerNumber;
	[Header("--- General Movement Variables ---")]
    public float moveSpeed;
    public float jumpForce;

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
    bool moving;
    bool grounded;
    bool falling;
    bool attackingMelee;
	bool hasWeapon = true;			// FIX ME: Hard coded to true temporarily to test animations

	// Numerical variables
	private float moveInput = 0.0f;

    // References
    Rigidbody2D rb;
	Transform transform;
    Transform sprites;
    Animator anim;
    Transform groundCheck;

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

			anim.SetFloat ("jabSpeed", jabSpeed);
			anim.SetTrigger ("jab");
		}
		// ... else use weapon as a club
		else 
		{
			anim.SetFloat ("clubPrepareSpeed",clubAttackSpeed);
			anim.SetFloat ("clubSpeed",clubAttackPepare);
			anim.SetTrigger ("club");
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
                return;
            }
        }
        grounded = false;
    }

    private void CheckFalling()
    {
        falling = rb.velocity.y < 0.0f;
    }

    public void OnCollisionEnter2DChild(Collision2D col)
    {
        Debug.Log("I am colliding with " + col.gameObject.name);
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
