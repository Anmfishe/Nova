using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPSOnEnter : MonoBehaviour {
    private ParticleSystem ps;
	// Use this for initialization
	void Start () {
        ps = transform.GetChild(0).GetComponent<ParticleSystem>();
        ps.Stop();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            ps.Play();
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            ps.Stop();
        }
    }
}
