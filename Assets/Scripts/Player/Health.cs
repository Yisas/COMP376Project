using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour
{
    private int nbOfLimbs;

	// Use this for initialization
	void Start ()
    {
        nbOfLimbs = 3;
	}
	
	// Update is called once per frame
	void Update () {
	    if(nbOfLimbs <= 0)
        {
            //kill
        }
	}

    public void TakeOffLimb()
    {
        if(nbOfLimbs == 3)
        {
            //take off arm
            nbOfLimbs--;
        }

        else if(nbOfLimbs == 2)
        {
            //take off leg
            nbOfLimbs--;
        }

        else if(nbOfLimbs == 1)
        {
            //take off head
            nbOfLimbs--;
        }
    }
}
