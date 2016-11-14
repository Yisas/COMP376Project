using UnityEngine;
using System.Collections;

public class FollowCam : MonoBehaviour {
    
    Transform follow;
    Transform other;
    public float smoothness;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        Move2();
	}

   public void Move1(Transform player) //follow just one player
    {
        //this is just for testing, make camera smothly follow living player
        transform.position = new Vector3(player.position.x, player.position.y, -10);
    }

   public void Move2() //follow both players
    {
        
        if (follow != null && other != null)
        {
            Vector2 between = new Vector2(follow.position.x + (other.position.x - follow.position.x) / 2, follow.position.y + (other.position.y - follow.position.y) / 2);
            Vector2 direction = new Vector2(between.x - GetComponent<Transform>().position.x, between.y - GetComponent<Transform>().position.y);
            GetComponent<Rigidbody2D>().AddForce(direction * smoothness, ForceMode2D.Impulse);
        }
       
    }

    public void Follow(Transform follow, Transform other)
    {
        this.follow = follow;
        this.other = other;
    }

    public void StopFollowing()
    {
        this.follow = null;
        this.other = null;
    }

}
