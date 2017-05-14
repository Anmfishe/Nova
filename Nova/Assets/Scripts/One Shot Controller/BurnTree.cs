using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurnTree : MonoBehaviour {
    private bool burning = false;
    private SpriteRenderer sr;
    private float burnRate = 0.002f;
    private ParticleSystem[] systems;
	// Use this for initialization
	void Start () {
        sr = GetComponent<SpriteRenderer>();
        systems = GetComponentsInChildren<ParticleSystem>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if(burning)
        {
            sr.color = new Color(sr.color.r - burnRate, sr.color.b - burnRate,
                sr.color.g - burnRate, 1);
        }
	}
    public void Burn()
    {
        burning = true;
        foreach(ParticleSystem sr in systems)
        {
            sr.Play();
        }
    }
}
