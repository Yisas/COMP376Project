using UnityEngine;
using System.Collections;

public class MusicManager : MonoBehaviour {

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void DetroyMusicManager()
    {
        Destroy(gameObject);
    }
}
