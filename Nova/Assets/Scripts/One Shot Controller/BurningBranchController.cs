using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurningBranchController : MonoBehaviour {
    private SpriteRenderer sr;
    public ParticleSystem ps;
    public SpriteRenderer[] flames;
    private float burnRate = 0.001f;
    private bool burning;
	// Use this for initialization
	void Start () {
        sr = GetComponent<SpriteRenderer>();
        ps.Stop();
    }
	
	// Update is called once per frame
	void FixedUpdate () {
		if(burning)
        {
            sr.color = new Color(sr.color.r - burnRate, sr.color.b - burnRate, sr.color.g - burnRate,
                sr.color.a);
            foreach(SpriteRenderer fsr in flames)
            {
                fsr.color = new Color(1, 1, 1, fsr.color.a + burnRate * 2);
            }
            ps.emissionRate = ps.emissionRate + 0.05f;
            
            if (sr.color.r <= 0)
            {
                burning = false;
            }
        }
        
	}
    public void BurnIt()
    {
        if(!burning)
        {
            ps.Play();
            burning = true;
        }
    }
}
