using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurnTree : MonoBehaviour {
    private bool burning = false;
    private SpriteRenderer sr;
    private float burnRate = 0.002f;
    private ParticleSystem[] systems;
    private AudioSource audS;
    private float targetVol;
	// Use this for initialization
	void Start () {
        sr = GetComponent<SpriteRenderer>();
        systems = GetComponentsInChildren<ParticleSystem>();
        audS = GameObject.FindGameObjectWithTag("FinalEmberSound").GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if(burning)
        {
            sr.color = new Color(sr.color.r - burnRate, sr.color.b - burnRate,
                sr.color.g - burnRate, 1);
            if(sr.color.r <= 0)
            {
                burning = false;
            }
            else if (audS != null && audS.volume < targetVol)
            {
                audS.volume += 0.005f;
            }
        }
        
        
        
	}
    public void Burn()
    {
        burning = true;
        foreach(ParticleSystem sr in systems)
        {
            sr.Play();
        }
        if(audS != null)
            targetVol = audS.volume + 0.14f;
    }
}
