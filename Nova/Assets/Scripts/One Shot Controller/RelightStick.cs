﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelightStick : MonoBehaviour {
    public FireNovaController fnc;
    public ParticleSystem ps;
    private CharacterController cc;
    private Camera cam;
    bool first = true;
    bool ready = false;
    int numColliders = 0;
	// Use this for initialization
	void Start () {
        cc = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterController>();
        fnc.anim.Play("FireNovaL2S3SpiderNest");
	}
	
	// Update is called once per frame
	void Update () {
        //if(!ps.isPlaying)
        //{
        //    
        //}
        if (!ps.isPlaying && first && cc.getDir() && Input.GetKey(KeyCode.E) && ready)
        {
            
            first = false;
            StartCoroutine(lightStick());
        }
    }
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            numColliders++;
            ready = true;
        }
    }
    void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            numColliders--;
            if(numColliders == 0)
            {
                //Debug.Log("Good");
                ready = false;
            }
            
        }
    }
    IEnumerator lightStick()
    {
        Debug.Log("Good");
        cc.canMove = false;
        yield return new WaitForSeconds(1);
        cc.anim.Play("NovaL2S3ReigniteTwig");
        yield return new WaitForSeconds(2);
        fnc.anim.Play("FireNovaL2S3SpiderNest2");
        yield return new WaitForSeconds(4);
        ps.Play();
        yield return new WaitForSeconds(2.75f);
        cc.canMove = true;
    }
}
