using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowRegrowth : MonoBehaviour {
    private GameObject player;
    private SpriteRenderer sr;
    private float fadeRate = 0.005f;
    private bool fadeIn = false;
    private Transform child;
	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        sr = GetComponent<SpriteRenderer>();
        //child = transform.GetChild(0);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if(fadeIn && sr.color.a < 1)
        {
            sr.color = new Color(1, 1, 1, sr.color.a + fadeRate);
        }
        else if(!fadeIn && sr.color.a > 0)
        {
            sr.color = new Color(1, 1, 1, sr.color.a - fadeRate);
        }
	}
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player" && !fadeIn)
            fadeIn = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && fadeIn)
            fadeIn = false;
    }
}
