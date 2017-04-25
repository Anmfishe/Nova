using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMusicOnEnter : MonoBehaviour {
    private AudioSource audioSource;
	// Use this for initialization
	void Start () {
        audioSource = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            audioSource.Play();
        }
    }
}
