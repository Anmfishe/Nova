using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {
    private AudioSource ads;
	// Use this for initialization
	void Start () {
        ads = GetComponent<AudioSource>();
        Application.LoadLevelAdditive("VerticalSlice");
    }

    // Update is called once per frame
    void Update () {
		
	}
    public void playSong()
    {
        if (!ads.isPlaying)
        {
            ads.Play();
        }
    }
    
}
