using UnityEngine;
using System.Collections;

public class ArrowCanvas : MonoBehaviour {

	private GameObject arrowLeft;
	private GameObject arrowRight;
	private GameObject pausedText;

	// Use this for initialization
	void Start () {
		arrowLeft = transform.FindChild ("Arrow Left").gameObject;
		arrowRight = transform.FindChild ("Arrow Right").gameObject;
		pausedText = transform.FindChild ("Paused Text").gameObject;
	}
	
	public void SetRightArrow(bool active)
	{
		arrowRight.SetActive (active);
	}

	public void SetLeftArrow(bool active)
	{
		arrowLeft.SetActive (active);
	}

	public void SetPasedText(bool active)
	{
		pausedText.SetActive (active);
	}
}

