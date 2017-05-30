﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropFireStick : MonoBehaviour {
    public GameObject fireStick;
    public GameObject player;
    public ParticleSystem fire;
    public SpriteRenderer flameAnim;
    public bool drop = true;
    private SpriteRenderer sr;
    bool first = true;
    bool blackenStick = false;
    float darkenRate = 0.001f;
	// Use this for initialization
	void Start () {
        sr = fireStick.GetComponent<SpriteRenderer>();
        player = GameObject.FindGameObjectWithTag("Player");
	}

    // Update is called once per frame
    void FixedUpdate()
    {
        if (blackenStick) {     
            if (sr.color.r > 0)
            {
                sr.color = new Color(sr.color.r - darkenRate, sr.color.b - darkenRate, sr.color.g - darkenRate, 1);
            }
            else
            {
                Destroy(gameObject);
            }
        }
	}
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player" && first)
        {
            first = false;
            fire.Stop();
            flameAnim.color = new Color(1, 1, 1, 0);
            if (drop)
            {

                StartCoroutine(dropAnim());
                blackenStick = true;
            }
        }
    }
    IEnumerator dropAnim()
    {
        player.GetComponent<CharacterController>().canMove = false;
        yield return new WaitForSeconds(0.5f);
        player.GetComponent<Animator>().Play("StickExtinguished");
        yield return new WaitForSeconds(3.5f);
        player.GetComponent<CharacterController>().canMove = true;
    }
    
}
