using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    public float moveSpeed;
    Vector3 direction;
    // Private references
    private Rigidbody2D rb;
	private Transform transform;
    Transform sprites;
    private Animator anim;

    private float moveInput = 0.0f;

	// Use this for initialization
	void Start ()
    {
        // Setup references
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        transform = GetComponent<Transform>();
        sprites = transform.FindChild("Sprites");
	}
	
	// Update is called once per frame
	void Update ()
    {
        CollectInput();
	}

    void FixedUpdate()
    {
        Move();
    }

    private void CollectInput()
    {
        moveInput = Input.GetAxis("Horizontal");
    }

    private void Move()
    {
        direction = new Vector2(moveInput, 0.0f);
        // Horizontal movement
        transform.Translate(direction * Time.deltaTime * moveSpeed);
        FaceDirection (direction);
        //rb.AddForce(new Vector2(moveInput * moveSpeed, 0));
        // Pass movement speed to animator
        anim.SetFloat("speed",Mathf.Abs(moveInput));
    }

	private void FaceDirection(Vector2 direction)
	{
        if (direction.x != 0.0f)
        {
            Quaternion rotation3D = direction.x > 0 ? Quaternion.LookRotation(Vector3.forward) : Quaternion.LookRotation(Vector3.back);
            sprites.rotation = rotation3D;
        }
    }
    
}
