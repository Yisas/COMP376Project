using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    public float moveSpeed;
    public float jumpForce;
    public float jabSpeed;
    public LayerMask mWhatIsGround;
    public float kGroundCheckRadius = 0.1f;
    Vector3 direction;

    // Booleans
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
        moveInput = Input.GetAxis("Horizontal");
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
    
}
