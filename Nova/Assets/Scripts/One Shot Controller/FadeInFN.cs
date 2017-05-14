using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeInFN : MonoBehaviour {
    public GameObject fireNova;
    private SpriteRenderer[] srs;
    private bool show;
    private float fadeInRate = 0.01f;
	// Use this for initialization
	void Start () {
        srs = fireNova.GetComponentsInChildren<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if(show)
        {
            foreach(SpriteRenderer sr in srs)
            {
                sr.color = new Color(1, 1, 1, sr.color.a + fadeInRate);
            }
        }
	}
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
            show = true;
    }
}
