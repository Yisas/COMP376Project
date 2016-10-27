using UnityEngine;
using System.Collections;

public class Throw : MonoBehaviour {

    private Rigidbody2D rb;

    [SerializeField] private float projectileSpeed;
	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(projectileSpeed, 0.0f); //testing purposes
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnCollisionEnter2D(Collision2D col)
    {
        if (rb.velocity.x != 0.0f)
        {
            rb.gravityScale = 10;
        }
            
    }
}
