using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableFloorScript : MonoBehaviour {
    private GameObject player;
    private Rigidbody2D rb2d;
    private AudioSource audS;
	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        rb2d = GetComponent<Rigidbody2D>();
        audS = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.tag == "Pushable")
        {
            gameObject.layer = LayerMask.NameToLayer("NoNovaCollision");
            rb2d.isKinematic = false;
            if(audS != null)
            {
                audS.Play();
            }
        }
    }

}
