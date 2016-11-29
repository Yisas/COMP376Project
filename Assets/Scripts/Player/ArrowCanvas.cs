using UnityEngine;
using System.Collections;

public class ArrowCanvas : MonoBehaviour {

	public GameObject arrowLeft;
	public GameObject arrowRight;

	// Use this for initialization
	void Start () {
		arrowLeft = transform.FindChild ("Arrow Left").gameObject;
		arrowRight = transform.FindChild ("Arrow Right").gameObject;
	}
	
	public void SetRightArrow(bool active)
	{
		arrowRight.SetActive (active);
	}

	public void SetLeftArrow(bool active)
	{
		arrowLeft.SetActive (active);
	}
}
