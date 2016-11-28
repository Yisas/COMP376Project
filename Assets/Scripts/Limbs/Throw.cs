﻿using UnityEngine;
using System.Collections;

public class Throw : MonoBehaviour {

    private Rigidbody2D rb;

    [SerializeField] private float projectileSpeed;

    public Transform armThrowTransform;
    public Vector2 limbDirection;
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
	        armThrowTransform.Translate(limbDirection*projectileSpeed, Space.World);
	        transform.RotateAround(armThrowTransform.position, Vector3.back*limbDirection.x, 20); 
	    }
	}

    public void ThrowLimb(Vector2 direction)
    {
		GameObject parentArmThrow = Instantiate(limbThrow, rotationPivot.position, transform.rotation) as GameObject;
        GameObject limbWeaponCopy = Instantiate(gameObject, gameObject.transform.position, gameObject.transform.rotation) as GameObject;
        
        if (parentArmThrow != null && limbWeaponCopy != null)
        {
            Throw throwComponent = limbWeaponCopy.GetComponent<Throw>();
            parentArmThrow.transform.SetParent(null);
            limbWeaponCopy.transform.SetParent(parentArmThrow.transform);
            limbWeaponCopy.transform.localScale = new Vector3(20, 20, 20);
            //transform.localRotation = Quaternion.Euler(180, 0 ,0);
            throwComponent.armThrowTransform = parentArmThrow.transform;
            throwComponent.limbDirection = direction;
            gameObject.SetActive(false);
            //parentArmThrow.transform.position += new Vector3(0.0f, 7.8f, 0.0f);
            throwComponent.SetIsThrow(true);
            limbWeaponCopy.GetComponent<Rigidbody2D>().isKinematic = false;
            limbWeaponCopy.GetComponent<Rigidbody2D>().gravityScale = 0;
        }
    }

    public bool LimbIsThrown()
    {
        return isThrown;
    }

    public void SetIsThrow(bool isThrown)
    {
        this.isThrown = isThrown;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (!(col.gameObject.GetComponent<PlayerController>() || col.gameObject.GetComponent<BonesPile>()))
        {
            Destroy(transform.parent.gameObject);
        }
    }
}
