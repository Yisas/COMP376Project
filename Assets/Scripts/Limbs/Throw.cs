using UnityEngine;
using System.Collections;

public class Throw : MonoBehaviour {

    private Rigidbody2D rb;

    [SerializeField] private float projectileSpeed;
    [SerializeField] private float angularVelocity;

    [Header("For testing purposes - Put this in the PlayerController")]
    public GameObject armThrow;

    // Use this for initialization
    void Start ()
    {
        // TODO: Make the limb spin when thrown.
        /*GameObject parentArmThrow = Instantiate(armThrow, transform.position + new Vector3(0.0f, 0.38f, 0.0f), Quaternion.identity) as GameObject;
        
        if (parentArmThrow != null)
        {
            parentArmThrow.transform.SetParent(null);
            gameObject.transform.SetParent(parentArmThrow.transform);
            parentArmThrow.GetComponent<Rigidbody2D>().angularVelocity = 180;
        }*/
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
            //rb.gravityScale = 10;
            Destroy(gameObject);
        }
            
    }
}
