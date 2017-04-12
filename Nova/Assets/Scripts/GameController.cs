using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {
    [HideInInspector]
    public bool loadLevel;
    public float musicFadeRate = 0.001f;
    private AudioSource ads;
    private bool fadeMusic = false;
	// Use this for initialization
	void Start () {
        ads = GetComponent<AudioSource>();
        if (loadLevel)
        {
            
            Application.LoadLevelAdditive("VerticalSlice");
        }
    }

    // Update is called once per frame
    void FixedUpdate () {
		if(fadeMusic)
        {
            if(ads.volume > 0)
            {
                ads.volume = ads.volume - musicFadeRate;
            }
            else
            {
                fadeMusic = false;
                ads.Stop();
            }
        }
	}
    public void playSong()
    {
        if (!ads.isPlaying)
        {
            ads.Play();
        }
    }
    public void stopSong()
    {
        fadeMusic = true;
    }
    
}
