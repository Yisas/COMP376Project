using UnityEngine;
using System.Collections;

public class Throw : MonoBehaviour {

    private Rigidbody2D rb;

    [SerializeField] private float projectileSpeed;

    private Transform armThrowTransform;
    private Vector2 limbDirection;
    private bool isThrown;

    public int playerNumber;

    [Header("For testing purposes - Put this in the PlayerController")]
    public GameObject limbThrow;

	private Transform rotationPivot;

    // Use this for initialization
    void Start ()
    {
		rotationPivot = transform.FindChild ("rotationPivot").transform;
        
    }
	
	// Update is called once per frame
	void Update () {
	    if (isThrown)
	    {
            print("Limb direction: " + limbDirection);
	        armThrowTransform.Translate(limbDirection*projectileSpeed, Space.World);
	        transform.RotateAround(armThrowTransform.position, Vector3.back*limbDirection.x, 20);
	    }
	}

    public void ThrowLimb(Vector2 direction)
    {
		GameObject parentArmThrow = Instantiate(limbThrow, rotationPivot.position, transform.rotation) as GameObject;

        if (parentArmThrow != null)
        {
            parentArmThrow.transform.SetParent(null);
            transform.SetParent(parentArmThrow.transform);
            //transform.localRotation = Quaternion.Euler(180, 0 ,0);
            armThrowTransform = parentArmThrow.transform;
            limbDirection = direction;
            //parentArmThrow.transform.position += new Vector3(0.0f, 7.8f, 0.0f);
            isThrown = true;
        }
    }

    public bool LimbIsThrown()
    {
        return isThrown;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (!col.gameObject.GetComponent<PlayerController>())
        {
            Destroy(gameObject);
        }
    }
}
