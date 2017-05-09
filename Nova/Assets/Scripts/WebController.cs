using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebController : MonoBehaviour {
    public bool showUIbutton = false;
    private BoxCollider2D bc2d;
    private SpriteRenderer sr;
    private ParticleSystem ps;
    bool burning;
    float darkenRate = 0.01f;
	// Use this for initialization
	void Start () {
        bc2d = GetComponent<BoxCollider2D>();
        sr = GetComponent<SpriteRenderer>();
        ps = GetComponentInChildren<ParticleSystem>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if(burning)
        {
            if (sr.color.r > 0)
                sr.color = new Color(sr.color.r - darkenRate, sr.color.b - darkenRate, sr.color.g - darkenRate, 1);
            else if (sr.color.a > 0)
                sr.color = new Color(0, 0, 0, sr.color.a - darkenRate);
            else
            {
                
                burning = false;
                ps.Stop();
            }
        }
	}
    public void burnAway()
    {
        bc2d.enabled = false;
        ps.Play();
        burning = true;
    }
}
