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
            print("I'm dead.");
        }
	}

    public void TakeOffLimb()
    {
        if(nbOfLimbs == 3)
        {
            //take off arm
            print("I took off arm");
            nbOfLimbs--;
        }

        else if(nbOfLimbs == 2)
        {
            //take off leg
            print("I took off leg");
            nbOfLimbs--;
        }

        else if(nbOfLimbs == 1)
        {
            //take off head
            print("I took off head");
            nbOfLimbs--;
        }
    }
}
