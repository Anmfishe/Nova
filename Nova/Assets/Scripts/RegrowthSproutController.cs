using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegrowthSproutController : MonoBehaviour {
    private RegrowthScript rgs;
    private ParticleSystem ps;
	// Use this for initialization
	void Start () {
        rgs = transform.parent.GetComponent<RegrowthScript>();
        ps = GetComponentInChildren<ParticleSystem>();
        
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && !rgs.didGrow)
        {
            ps.Play();
        }
    }
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Player" && rgs.didGrow)
        {
            ps.Stop();
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            ps.Stop();
        }
        if(rgs.didGrow)
        {
            Destroy(gameObject);
        }
    }
}
