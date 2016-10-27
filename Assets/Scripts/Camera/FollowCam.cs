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
        Move();
	}

    void Move()
    {
        
        if (follow != null && other != null)
        {
            Vector3 between = new Vector3(follow.position.x + (other.position.x - follow.position.x) / 2, follow.position.y + (other.position.y - follow.position.y) / 2, transform.position.z);
            transform.position = Vector3.Lerp(transform.position, between, smoothness * Time.deltaTime);
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
