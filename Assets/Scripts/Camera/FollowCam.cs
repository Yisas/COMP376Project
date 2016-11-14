﻿using UnityEngine;
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
