using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour {
    private GameObject gm;
    private GameController gc;
	// Use this for initialization
	void Start () {
        gm = transform.parent.gameObject;
        gc = gm.GetComponent<GameController>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            gc.playSong();
        }
    }
}
