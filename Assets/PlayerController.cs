using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    public float moveSpeed;

    // Private references
    private Rigidbody2D rb;
    private Animator anim;

    private float moveInput = 0.0f;

	// Use this for initialization
	void Start ()
    {
        // Setup references
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
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
        // Horizontal movement
        rb.AddForce(new Vector2(moveInput * moveSpeed, 0));
        // Pass movement speed to animator
        anim.SetFloat("speed",moveInput);
    }
}
